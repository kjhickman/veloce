using System.Diagnostics;
using ChessLite;
using ChessLite.Movement;
using Veloce.Engine;
using Veloce.Evaluation;

namespace Veloce.Search;

public sealed class NegamaxSearch
{
    private const int MateScore = 100_000;
    private const int MaxQuiescencePly = 8;
    private long _nodes;

    public SearchResult FindBestMove(Game game, SearchSettings settings, CancellationToken cancellationToken = default)
    {
        var now = Stopwatch.GetTimestamp();
        _nodes = 0;

        using var timeLimit = settings.MoveTime.HasValue ? new CancellationTokenSource(settings.MoveTime.Value) : null;
        using var linkedCancellation = timeLimit is null
            ? null
            : CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeLimit.Token);
        var effectiveCancellation = linkedCancellation?.Token ?? cancellationToken;

        if (game.GetDrawState().IsDraw())
        {
            return new SearchResult(null, 0, 0, _nodes, Stopwatch.GetElapsedTime(now));
        }

        Span<Move> moves = stackalloc Move[218];
        var moveCount = game.WriteLegalMoves(moves);
        if (moveCount == 0)
        {
            return new SearchResult(null, EvaluateTerminal(game, 0), 0, _nodes, Stopwatch.GetElapsedTime(now));
        }

        var bestMove = moves[0];
        var bestScore = MaterialEvaluator.Evaluate(game.Position);
        var completedDepth = 0;

        for (var depth = 1; depth <= settings.Depth; depth++)
        {
            try
            {
                effectiveCancellation.ThrowIfCancellationRequested();

                var depthBestMove = bestMove;
                var depthBestScore = int.MinValue + 1;
                var alpha = int.MinValue + 1;
                const int beta = int.MaxValue;

                for (var i = 0; i < moveCount; i++)
                {
                    effectiveCancellation.ThrowIfCancellationRequested();

                    var move = moves[i];
                    game.MakeMove(move);
                    int score;
                    try
                    {
                        score = -Search(game, depth - 1, -beta, -alpha, 1, effectiveCancellation);
                    }
                    finally
                    {
                        game.UndoMove();
                    }

                    if (score > depthBestScore)
                    {
                        depthBestScore = score;
                        depthBestMove = move;
                    }

                    if (score > alpha)
                    {
                        alpha = score;
                    }
                }

                bestMove = depthBestMove;
                bestScore = depthBestScore;
                completedDepth = depth;
            }
            catch (OperationCanceledException) when (effectiveCancellation.IsCancellationRequested)
            {
                break;
            }
        }

        return new SearchResult(bestMove, bestScore, completedDepth, _nodes, Stopwatch.GetElapsedTime(now));
    }

    private int Search(Game game, int depth, int alpha, int beta, int ply, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _nodes++;

        if (game.GetDrawState().IsDraw())
        {
            return 0;
        }

        if (depth == 0)
        {
            return Quiescence(game, alpha, beta, ply, 0, cancellationToken);
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
            int score;
            try
            {
                score = -Search(game, depth - 1, -beta, -alpha, ply + 1, cancellationToken);
            }
            finally
            {
                game.UndoMove();
            }

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

    private int Quiescence(Game game, int alpha, int beta, int ply, int qPly, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _nodes++;

        if (game.GetDrawState().IsDraw())
        {
            return 0;
        }

        if (qPly >= MaxQuiescencePly)
        {
            return MaterialEvaluator.Evaluate(game.Position);
        }

        var inCheck = game.IsInCheck();
        if (!inCheck)
        {
            var standPat = MaterialEvaluator.Evaluate(game.Position);
            if (standPat >= beta)
            {
                return beta;
            }

            if (standPat > alpha)
            {
                alpha = standPat;
            }
        }

        Span<Move> moves = stackalloc Move[218];
        var moveCount = game.WriteLegalMoves(moves);
        if (moveCount == 0)
        {
            return EvaluateTerminal(game, ply);
        }

        for (var i = 0; i < moveCount; i++)
        {
            var move = moves[i];
            if (!inCheck && !move.IsCapture)
            {
                continue;
            }

            game.MakeMove(move);
            int score;
            try
            {
                score = -Quiescence(game, -beta, -alpha, ply + 1, qPly + 1, cancellationToken);
            }
            finally
            {
                game.UndoMove();
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

        return alpha;
    }

    private static int EvaluateTerminal(Game game, int ply)
    {
        return game.IsInCheck() ? -MateScore + ply : 0;
    }
}
