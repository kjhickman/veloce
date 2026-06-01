using Veloce.Engine;

namespace Veloce.Uci;

internal sealed class UciSearchSession
{
    private readonly Lock _stateLock = new();
    private readonly Lock _consoleLock = new();
    private Task? _activeSearch;
    private CancellationTokenSource? _activeSearchCancellation;
    private int _activeSearchId;

    public async ValueTask StartAsync(Func<CancellationToken, SearchResult> search)
    {
        await StopAsync();

        lock (_stateLock)
        {
            _activeSearchCancellation = new CancellationTokenSource();
            var searchCancellation = _activeSearchCancellation;
            var searchId = ++_activeSearchId;
            _activeSearch = Task.Run(() => CompleteSearchAsync(search, searchCancellation, searchId), CancellationToken.None);
        }
    }

    public async ValueTask StopAsync()
    {
        Task? activeSearch;
        lock (_stateLock)
        {
            _activeSearchCancellation?.Cancel();
            activeSearch = _activeSearch;
        }

        if (activeSearch is null)
        {
            return;
        }

        await activeSearch;
    }

    private Task CompleteSearchAsync(Func<CancellationToken, SearchResult> search, CancellationTokenSource searchCancellation, int searchId)
    {
        try
        {
            var result = search(searchCancellation.Token);
            lock (_consoleLock)
            {
                Console.WriteLine($"info depth {result.Depth} score cp {result.Score} nodes {result.Nodes} time {(long)result.Elapsed.TotalMilliseconds}");
                Console.WriteLine($"bestmove {(result.BestMove.HasValue ? UciMoveFormatter.Format(result.BestMove.Value) : "0000")}");
                Console.Out.Flush();
            }
        }
        finally
        {
            lock (_stateLock)
            {
                if (searchId == _activeSearchId && ReferenceEquals(searchCancellation, _activeSearchCancellation))
                {
                    _activeSearch = null;
                    _activeSearchCancellation = null;
                    searchCancellation.Dispose();
                }
            }
        }

        return Task.CompletedTask;
    }
}
