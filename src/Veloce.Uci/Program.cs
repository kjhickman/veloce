using Veloce.Engine;

namespace Veloce.Uci;

public static class Program
{
    public static async Task Main()
    {
        var engine = new VeloceEngine();
        var output = new UciOutput();
        var searchSession = new UciSearchSession(output);

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
                    output.WriteLine("option name Threads type spin default 1 min 1 max 1");
                    output.WriteLine("option name Hash type spin default 16 min 1 max 1024");
                    output.WriteLine("uciok");
                    break;

                case "setoption":
                    await searchSession.StopAsync();
                    SetOptionCommand.Handle(engine, parts);
                    break;

                case "isready":
                    output.WriteLine("readyok");
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

            output.Flush();
        }
    }
}
