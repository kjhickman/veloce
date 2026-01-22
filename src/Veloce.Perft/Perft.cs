using ChessLite;
using ChessLite.Movement;
using ChessLite.State;
using Veloce.Core;

namespace Veloce.Perft;

public static class Perft
{
    public static long CountNodes(Game game, int depth)
    {
        if (depth == 0) return 1;

        Span<Move> moves = stackalloc Move[218]; // Max possible moves
        var moveCount = game.WriteLegalMoves(moves);

        if (depth == 1) return moveCount;

        long nodes = 0;
        for (var i = 0; i < moveCount; i++)
        {
            game.MakeMove(moves[i]);
            nodes += CountNodes(game, depth - 1);
            game.UndoMove();
        }

        return nodes;
    }

    // Method for debugging specific positions
    public static Dictionary<string, long> DividePerft(Game game, int depth)
    {
        var result = new Dictionary<string, long>();
        Span<Move> moves = stackalloc Move[218];
        var moveCount = game.WriteLegalMoves(moves);

        for (var i = 0; i < moveCount; i++)
        {
            var moveStr = moves[i].ToString();
            game.MakeMove(moves[i]);
            var nodes = CountNodes(game, depth - 1);
            game.UndoMove();
            result[moveStr] = nodes;
        }

        return result;
    }
}
