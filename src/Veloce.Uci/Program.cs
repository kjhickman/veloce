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
                    var bestMove = engine.FindBestMove();
                    Console.WriteLine("info depth 1 score cp 0 nodes 1 time 0");
                    Console.WriteLine($"bestmove {(bestMove.HasValue ? UciMoveFormatter.Format(bestMove.Value) : "0000")}");
                    break;

                case "stop":
                    break;

                case "quit":
                    return;
            }

            Console.Out.Flush();
        }
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
