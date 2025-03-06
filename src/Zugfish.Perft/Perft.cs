using Zugfish.Engine;
using Zugfish.Engine.Models;

namespace Zugfish.Perft;

public static class Perft
{
    public static long CountNodes(Position position, MoveExecutor executor, int depth)
    {
        if (depth == 0) return 1;

        Span<Move> moves = stackalloc Move[218]; // Max possible moves
        var moveCount = MoveGeneration.GenerateLegalMoves(position, moves);

        if (depth == 1) return moveCount;

        long nodes = 0;
        for (var i = 0; i < moveCount; i++)
        {
            executor.MakeMove(position, moves[i]);
            nodes += CountNodes(position, executor, depth - 1);
            executor.UndoMove(position);
        }

        return nodes;
    }

    // Method for debugging specific positions
    public static Dictionary<string, long> DividePerft(Position position, MoveExecutor executor, int depth)
    {
        var result = new Dictionary<string, long>();
        Span<Move> moves = stackalloc Move[218];
        var moveCount = MoveGeneration.GenerateLegalMoves(position, moves);

        for (var i = 0; i < moveCount; i++)
        {
            var moveStr = moves[i].ToString();
            executor.MakeMove(position, moves[i]);
            var nodes = CountNodes(position, executor, depth - 1);
            executor.UndoMove(position);
            result[moveStr] = nodes;
        }

        return result;
    }
}
