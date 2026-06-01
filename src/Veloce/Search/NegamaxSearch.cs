using System.Diagnostics;
using ChessLite;
using ChessLite.Movement;
using Veloce.Engine;
using Veloce.Evaluation;
using Veloce.Search.Transposition;

namespace Veloce.Search;

public sealed class NegamaxSearch
{
    private const int MateScore = 100_000;
    private const int MateThreshold = MateScore - 1_000;
    private const int MaxQuiescencePly = 8;
    private const ulong HalfmoveHashMultiplier = 0x9E37_79B9_7F4A_7C15UL;
    private readonly TranspositionTable _transpositions = new();
    private long _nodes;

    public void SetHashSize(int megabytes)
    {
        _transpositions.Resize(megabytes);
    }

    public SearchResult FindBestMove(Game game, SearchSettings settings, CancellationToken cancellationToken = default)
    {
        var now = Stopwatch.GetTimestamp();
        _nodes = 0;

        using var timeLimit = settings.MoveTime.HasValue ? new CancellationTokenSource(settings.MoveTime.Value) : null;
        using var linkedCancellation = timeLimit is null
            ? null
            : CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeLimit.Token);
        var effectiveCancellation = linkedCancellation?.Token ?? cancellationToken;

        _transpositions.NewSearch();

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
        var rootKey = GetTranspositionKey(game);

        for (var depth = 1; depth <= settings.Depth; depth++)
        {
            try
            {
                effectiveCancellation.ThrowIfCancellationRequested();

                var depthBestMove = bestMove;
                var depthBestScore = int.MinValue + 1;
                var alpha = int.MinValue + 1;
                const int beta = int.MaxValue;

                if (_transpositions.TryGet(rootKey, out var rootEntry))
                {
                    MoveToFront(moves, moveCount, rootEntry.Move.FindMatchingMove(moves, moveCount));
                }

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
                _transpositions.Store(rootKey, ScoreToTable(bestScore, 0), new CompactMove(bestMove), depth, TranspositionBound.Exact);
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

        var originalAlpha = alpha;
        var key = GetTranspositionKey(game);
        if (_transpositions.TryGet(key, out var entry))
        {
            if (entry.Depth >= depth)
            {
                var score = ScoreFromTable(entry.Score, ply);
                if (entry.Bound == TranspositionBound.Exact)
                {
                    return score;
                }

                if (entry.Bound == TranspositionBound.Lower && score >= beta)
                {
                    return score;
                }

                if (entry.Bound == TranspositionBound.Upper && score <= alpha)
                {
                    return score;
                }
            }
        }

        Span<Move> moves = stackalloc Move[218];
        var moveCount = game.WriteLegalMoves(moves);
        if (moveCount == 0)
        {
            return EvaluateTerminal(game, ply);
        }

        if (_transpositions.TryGet(key, out entry))
        {
            MoveToFront(moves, moveCount, entry.Move.FindMatchingMove(moves, moveCount));
        }

        var bestScore = int.MinValue + 1;
        var bestMove = Move.NullMove;

        for (var i = 0; i < moveCount; i++)
        {
            var move = moves[i];
            game.MakeMove(move);
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
                bestMove = move;
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

        var bound = bestScore <= originalAlpha
            ? TranspositionBound.Upper
            : bestScore >= beta
                ? TranspositionBound.Lower
                : TranspositionBound.Exact;
        _transpositions.Store(key, ScoreToTable(bestScore, ply), new CompactMove(bestMove), depth, bound);

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

    private static ulong GetTranspositionKey(Game game)
    {
        var key = game.Position.ZobristHash ^ ((ulong)game.Position.HalfmoveClock * HalfmoveHashMultiplier);
        return key == 0 ? 1 : key;
    }

    private static int ScoreToTable(int score, int ply)
    {
        if (score > MateThreshold) return score + ply;
        if (score < -MateThreshold) return score - ply;
        return score;
    }

    private static int ScoreFromTable(int score, int ply)
    {
        if (score > MateThreshold) return score - ply;
        if (score < -MateThreshold) return score + ply;
        return score;
    }

    private static void MoveToFront(Span<Move> moves, int moveCount, Move move)
    {
        if (move == Move.NullMove)
        {
            return;
        }

        for (var i = 0; i < moveCount; i++)
        {
            if (moves[i] != move)
            {
                continue;
            }

            (moves[0], moves[i]) = (moves[i], moves[0]);
            return;
        }
    }
}
