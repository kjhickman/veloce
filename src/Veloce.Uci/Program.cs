using ChessLite.Parsing;
using ChessLite.State;
using Veloce.Engine;

namespace Veloce.Uci;

public static class Program
{
    public static void Main()
    {
        var engine = new VeloceEngine();

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
                    engine.NewGame();
                    break;

                case "position":
                    SetPosition(engine, parts);
                    break;

                case "go":
                    var result = engine.FindBestMove(ParseSearchSettings(parts));
                    Console.WriteLine($"info depth {result.Depth} score cp {result.Score} nodes {result.Nodes} time {(long)result.Elapsed.TotalMilliseconds}");
                    Console.WriteLine($"bestmove {(result.BestMove.HasValue ? UciMoveFormatter.Format(result.BestMove.Value) : "0000")}");
                    break;

                case "stop":
                    break;

                case "quit":
                    return;
            }

            Console.Out.Flush();
        }
    }

    private static SearchSettings ParseSearchSettings(string[] commandParts)
    {
        var depth = SearchSettings.Default.Depth;

        for (var i = 1; i < commandParts.Length - 1; i++)
        {
            if (commandParts[i].Equals("depth", StringComparison.OrdinalIgnoreCase)
                && int.TryParse(commandParts[i + 1], out var parsedDepth))
            {
                depth = Math.Max(1, parsedDepth);
                break;
            }
        }

        return new SearchSettings(depth);
    }

    private static void SetPosition(VeloceEngine engine, string[] commandParts)
    {
        if (commandParts.Length < 2) return;

        var index = 1;
        Position position;

        if (commandParts[index].Equals("startpos", StringComparison.OrdinalIgnoreCase))
        {
            position = new Position();
            index++;
        }
        else if (commandParts[index].Equals("fen", StringComparison.OrdinalIgnoreCase) && index + 1 < commandParts.Length)
        {
            var fenBuilder = new System.Text.StringBuilder();
            index++;

            while (index < commandParts.Length && !commandParts[index].Equals("moves", StringComparison.OrdinalIgnoreCase))
            {
                fenBuilder.Append(commandParts[index++]);
                fenBuilder.Append(' ');
            }

            position = Fen.Parse(fenBuilder.ToString().Trim());
        }
        else
        {
            return;
        }

        engine.SetPosition(position);

        if (index >= commandParts.Length || !commandParts[index].Equals("moves", StringComparison.OrdinalIgnoreCase)) return;

        index++;
        while (index < commandParts.Length)
        {
            try
            {
                engine.MakeUciMove(commandParts[index++]);
            }
            catch (ArgumentException)
            {
                break;
            }
        }
    }
}
