using System.Diagnostics;
using ChessLite;
using ChessLite.Movement;
using Veloce.Engine;
using Veloce.Evaluation;

namespace Veloce.Search;

public sealed class NegamaxSearch
{
    private const int MateScore = 100_000;
    private long _nodes;

    public SearchResult FindBestMove(Game game, SearchSettings settings)
    {
        var now = Stopwatch.GetTimestamp();
        _nodes = 0;

        Span<Move> moves = stackalloc Move[218];
        var moveCount = game.WriteLegalMoves(moves);
        if (moveCount == 0)
        {
            return new SearchResult(null, EvaluateTerminal(game, 0), settings.Depth, _nodes, Stopwatch.GetElapsedTime(now));
        }

        var bestMove = moves[0];
        var bestScore = int.MinValue + 1;
        var alpha = int.MinValue + 1;
        const int beta = int.MaxValue;

        for (var i = 0; i < moveCount; i++)
        {
            var move = moves[i];
            game.MakeMove(move);
            var score = -Search(game, settings.Depth - 1, -beta, -alpha, 1);
            game.UndoMove();

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }

            if (score > alpha)
            {
                alpha = score;
            }
        }

        return new SearchResult(bestMove, bestScore, settings.Depth, _nodes, Stopwatch.GetElapsedTime(now));
    }

    private int Search(Game game, int depth, int alpha, int beta, int ply)
    {
        _nodes++;

        if (depth == 0)
        {
            return MaterialEvaluator.Evaluate(game.Position);
        }

        Span<Move> moves = stackalloc Move[218];
        var moveCount = game.WriteLegalMoves(moves);
        if (moveCount == 0)
        {
            return EvaluateTerminal(game, ply);
        }

        var bestScore = int.MinValue + 1;

        for (var i = 0; i < moveCount; i++)
        {
            game.MakeMove(moves[i]);
            var score = -Search(game, depth - 1, -beta, -alpha, ply + 1);
            game.UndoMove();

            if (score > bestScore)
            {
                bestScore = score;
            }

            if (score > alpha)
            {
                alpha = score;
            }

            if (alpha >= beta)
            {
                break;
            }
        }

        return bestScore;
    }

    private static int EvaluateTerminal(Game game, int ply)
    {
        return game.IsInCheck() ? -MateScore + ply : 0;
    }
}
