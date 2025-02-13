using Zugfish.Engine;

namespace Zugfish.UCI;

public static class Program
{
    public static void Main()
    {
        var board = new Board();
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
                    board = new Board();
                    break;

                case "position":
                    if (parts.Length < 2) break;
                    
                    if (parts[1] == "startpos")
                    {
                        board = new Board();
                        var movesIndex = Array.IndexOf(parts, "moves");
                        if (movesIndex != -1)
                        {
                            // Apply all moves after "moves"
                            for (var i = movesIndex + 1; i < parts.Length; i++)
                            {
                                board.MakeMove(parts[i]);
                            }
                        }
                    }
                    else if (parts[1] == "fen" && parts.Length >= 7)
                    {
                        var fen = string.Join(" ", parts.Skip(2).Take(6));
                        board = new Board(fen);
                        
                        var movesIndex = Array.IndexOf(parts, "moves");
                        if (movesIndex != -1)
                        {
                            // Apply all moves after "moves"
                            for (var i = movesIndex + 1; i < parts.Length; i++)
                            {
                                board.MakeMove(parts[i]);
                            }
                        }
                    }
                    break;

                case "go":
                    // Using depth 4 as a default - this can be made configurable later
                    var bestMove = Search.FindBestMove(moveGenerator, board, 4);
                    if (bestMove.HasValue)
                    {
                        Console.WriteLine($"bestmove {Helpers.UciFromMove(bestMove.Value)}");
                    }
                    else
                    {
                        Console.WriteLine("bestmove 0000"); // null move in case of no legal moves
                    }
                    break;

                default:
                    Console.WriteLine($"Unknown command: {command}");
                    break;
            }
        }
    }
}

