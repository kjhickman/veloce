using Veloce.Engine;

namespace Veloce.Uci;

public static class Program
{
    public static async Task Main()
    {
        var engine = new VeloceEngine();
        var searchSession = new UciSearchSession();

        while (true)
        {
            var line = Console.ReadLine()?.Trim();
            if (string.IsNullOrEmpty(line)) continue;

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var command = parts[0].ToLowerInvariant();

            switch (command)
            {
                case "uci":
                    Console.WriteLine("id name Veloce");
                    Console.WriteLine("id author Kyle Hickman");
                    Console.WriteLine("option name Threads type spin default 1 min 1 max 1");
                    Console.WriteLine("option name Hash type spin default 16 min 1 max 16");
                    Console.WriteLine("uciok");
                    break;

                case "setoption":
                    break;

                case "isready":
                    Console.WriteLine("readyok");
                    break;

                case "ucinewgame":
                    await searchSession.StopAsync();
                    engine.NewGame();
                    break;

                case "position":
                    await searchSession.StopAsync();
                    PositionCommand.Handle(engine, parts);
                    break;

                case "go":
                    await GoCommand.HandleAsync(engine, searchSession, parts);
                    break;

                case "stop":
                    await searchSession.StopAsync();
                    break;

                case "quit":
                    await searchSession.StopAsync();
                    return;
            }

            Console.Out.Flush();
        }
    }
}
