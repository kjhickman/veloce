using Veloce.Engine;
using Veloce.Uci.Commands;

namespace Veloce.Uci;

public static class Program
{
    public static async Task Main()
    {
        var engine = new VeloceEngine();
        var output = new UciWriter();
        var session = new SearchSession(output);

        while (true)
        {
            var line = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(line)) continue;

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var command = parts[0].ToLowerInvariant();

            switch (command)
            {
                case "uci":
                    output.WriteLine("id name Veloce");
                    output.WriteLine("id author Kyle Hickman");
                    output.WriteLine($"option name Threads type spin default 1 min 1 max {VeloceEngine.MaximumThreadCount}");
                    output.WriteLine("option name Hash type spin default 16 min 1 max 1024");
                    output.WriteLine("option name Clear Hash type button");
                    output.WriteLine("option name MultiPV type spin default 1 min 1 max 1");
                    output.WriteLine("option name UCI_AnalyseMode type check default false");
                    output.WriteLine("option name Ponder type check default false");
                    output.WriteLine("uciok");
                    break;

                case "setoption":
                    await session.StopAsync();
                    SetOptionCommand.Handle(engine, parts);
                    break;

                case "isready":
                    output.WriteLine("readyok");
                    break;

                case "ucinewgame":
                    await session.StopAsync();
                    engine.NewGame();
                    break;

                case "position":
                    await session.StopAsync();
                    PositionCommand.Handle(engine, parts);
                    break;

                case "go":
                    await GoCommand.HandleAsync(engine, session, output, parts);
                    break;

                case "ponderhit":
                    {
                        var pendingSettings = session.PendingPonderSettings;
                        if (pendingSettings is not null)
                        {
                            session.PendingPonderSettings = null;
                            await session.StopAsync(emitBestMove: false);
                            await session.StartAsync((cancellationToken, searchInfo) => engine.FindBestMove(pendingSettings, searchInfo, cancellationToken));
                        }
                    }

                    break;

                case "stop":
                    await session.StopAsync();
                    break;

                case "quit":
                    await session.StopAsync();
                    return;
            }

            output.Flush();
        }
    }
}
