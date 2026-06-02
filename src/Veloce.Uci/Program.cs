using System.Threading.Channels;
using Veloce.Engine;

namespace Veloce.Uci;

public static class Program
{
    public static async Task Main()
    {
        var channel = Channel.CreateUnbounded<UciEvent>(new UnboundedChannelOptions { SingleReader = true });
        var engine = new VeloceEngine();
        Task<SearchResult>? activeSearch = null;
        CancellationTokenSource? activeSearchCancellation = null;
        var activeSearchId = 0;

        _ = Task.Run(async () =>
        {
            while (true)
            {
                var line = Console.ReadLine()?.Trim();
                if (line is null) break;
                if (line.Length == 0) continue;

                await channel.Writer.WriteAsync(UciEvent.Input(line));
            }

            channel.Writer.TryComplete();
        });

        await foreach (var uciEvent in channel.Reader.ReadAllAsync())
        {
            switch (uciEvent)
            {
                case { Kind: UciEventKind.SearchInfo } searchInfo:
                    if (searchInfo.SearchId == activeSearchId && activeSearch is not null)
                    {
                        WriteInfo(searchInfo.SearchInfo);
                    }

                    break;

                case { Kind: UciEventKind.SearchCompleted } completed:
                    if (completed.SearchId == activeSearchId && activeSearch is not null && completed.SearchResult is not null)
                    {
                        CompleteSearch(completed.SearchResult);
                    }

                    break;

                case { Kind: UciEventKind.Input } input:
                    var parts = input.Line!.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var command = parts[0].ToLowerInvariant();

                    switch (command)
                    {
                        case "uci":
                            Console.WriteLine("id name Veloce");
                            Console.WriteLine("id author Kyle Hickman");
                            Console.WriteLine("option name Threads type spin default 1 min 1 max 1");
                            Console.WriteLine("option name Hash type spin default 16 min 1 max 1024");
                            Console.WriteLine("uciok");
                            break;

                        case "setoption":
                            await StopActiveSearchAsync();
                            SetOptionCommand.Handle(engine, parts);
                            break;

                        case "isready":
                            Console.WriteLine("readyok");
                            break;

                        case "ucinewgame":
                            await StopActiveSearchAsync();
                            engine.NewGame();
                            break;

                        case "position":
                            await StopActiveSearchAsync();
                            PositionCommand.Handle(engine, parts);
                            break;

                        case "go":
                            await StopActiveSearchAsync();
                            StartSearch(GoCommand.CreateSearchSettings(engine, parts));
                            break;

                        case "stop":
                            await StopActiveSearchAsync();
                            break;

                        case "quit":
                            await StopActiveSearchAsync();
                            return;
                    }

                    Console.Out.Flush();
                    break;
            }
        }

        return;

        void StartSearch(SearchSettings settings)
        {
            activeSearchCancellation = new CancellationTokenSource();
            var searchCancellation = activeSearchCancellation;
            var searchId = ++activeSearchId;
            activeSearch = Task.Run(
                () => engine.FindBestMove(
                    settings,
                    info => channel.Writer.TryWrite(UciEvent.Info(searchId, info)),
                    searchCancellation.Token),
                CancellationToken.None);
            _ = activeSearch.ContinueWith(
                task => channel.Writer.TryWrite(UciEvent.Completed(searchId, task.Result)),
                CancellationToken.None,
                TaskContinuationOptions.ExecuteSynchronously | TaskContinuationOptions.OnlyOnRanToCompletion,
                TaskScheduler.Default);
        }

        async ValueTask StopActiveSearchAsync()
        {
            if (activeSearch is null)
            {
                return;
            }

            activeSearchCancellation?.Cancel();
            var result = await activeSearch;
            CompleteSearch(result);
        }

        void CompleteSearch(SearchResult result)
        {
            WriteInfo(result);
            Console.WriteLine($"bestmove {(result.BestMove.HasValue ? UciMoveFormatter.Format(result.BestMove.Value) : "0000")}");
            Console.Out.Flush();
            activeSearchCancellation?.Dispose();
            activeSearchCancellation = null;
            activeSearch = null;
        }
    }

    private static void WriteInfo(SearchInfo info)
    {
        Console.WriteLine($"info depth {info.Depth} score cp {info.Score} nodes {info.Nodes} time {(long)info.Elapsed.TotalMilliseconds}");
    }

    private static void WriteInfo(SearchResult result)
    {
        Console.WriteLine($"info depth {result.Depth} score cp {result.Score} nodes {result.Nodes} time {(long)result.Elapsed.TotalMilliseconds}");
    }

    private readonly struct UciEvent
    {
        private UciEvent(UciEventKind kind, string? line, int searchId, SearchInfo searchInfo, SearchResult? searchResult)
        {
            Kind = kind;
            Line = line;
            SearchId = searchId;
            SearchInfo = searchInfo;
            SearchResult = searchResult;
        }

        public UciEventKind Kind { get; }

        public string? Line { get; }

        public int SearchId { get; }

        public SearchInfo SearchInfo { get; }

        public SearchResult? SearchResult { get; }

        public static UciEvent Input(string line) => new(UciEventKind.Input, line, 0, default, null);

        public static UciEvent Info(int searchId, SearchInfo info) => new(UciEventKind.SearchInfo, null, searchId, info, null);

        public static UciEvent Completed(int searchId, SearchResult result) => new(UciEventKind.SearchCompleted, null, searchId, default, result);
    }

    private enum UciEventKind
    {
        Input,
        SearchInfo,
        SearchCompleted,
    }
}
