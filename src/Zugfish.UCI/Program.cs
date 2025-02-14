using Zugfish.Engine;
using Zugfish.UCI.Lib;

namespace Zugfish.UCI;

public static class Program
{
    public static void Main()
    {
        var position = new Position();
        var moveGenerator = new MoveGenerator();

        while (true)
        {
            var line = Console.ReadLine()?.Trim().ToLower();
            if (string.IsNullOrEmpty(line)) continue;

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var command = parts[0];

            switch (command)
            {
                case "uci":
                    // Identify the engine
                    Console.WriteLine("id name Zugfish");
                    Console.WriteLine("id author Kyle");
                    // Add options here later
                    Console.WriteLine("uciok");
                    break;

                case "isready":
                    Console.WriteLine("readyok");
                    break;

                case "quit":
                    return;

                case "ucinewgame":
                    position = new Position();
                    break;

                case "position":
                    var newPosition = Helpers.ParsePosition(parts);
                    if (newPosition != null) position = newPosition;
                    break;

                case "go":
                    // Using depth 4 as a default - this can be made configurable later
                    var bestMove = Search.FindBestMove(moveGenerator, position, 4);
                    var bestMoveString = bestMove.HasValue ? Helpers.UciFromMove(bestMove.Value) : "0000";
                    Console.WriteLine($"bestmove {bestMoveString}");
                    break;

                default:
                    Console.WriteLine($"Unknown command: {command}");
                    break;
            }
        }
    }
}

