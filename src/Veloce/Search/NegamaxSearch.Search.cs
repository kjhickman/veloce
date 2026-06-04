using ChessLite;
using ChessLite.Movement;
using Veloce.Evaluation;
using Veloce.Search.Transposition;

namespace Veloce.Search;

public sealed partial class NegamaxSearch
{
    private int Search(Game game, int depth, int alpha, int beta, int ply, bool allowNullMove, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        RecordNode();

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

        var inCheck = game.IsInCheck();
        if (ShouldTryNullMove(game, depth, alpha, beta, allowNullMove, inCheck))
        {
            game.MakeNullMove();
            int score;
            try
            {
                score = -Search(game, depth - 1 - NullMoveReduction, -beta, -beta + 1, ply + 1, false, cancellationToken);
            }
            finally
            {
                game.UndoMove();
            }

            if (score >= beta)
            {
                return beta;
            }
        }

        Span<Move> moves = stackalloc Move[218];
        var moveCount = game.WriteLegalMoves(moves);
        if (moveCount == 0)
        {
            return EvaluateTerminal(game, ply);
        }

        var tableMove = _transpositions.TryGet(key, out entry)
            ? entry.Move.FindMatchingMove(moves, moveCount)
            : Move.NullMove;

        var bestScore = int.MinValue + 1;
        var bestMove = Move.NullMove;
        var searchedMoves = 0;

        for (var i = 0; i < moveCount; i++)
        {
            var move = PickNextMove(moves, moveCount, i, tableMove, ply, useKillers: true);
            game.MakeMove(move);
            int score;
            try
            {
                if (searchedMoves == 0)
                {
                    score = -Search(game, depth - 1, -beta, -alpha, ply + 1, true, cancellationToken);
                }
                else
                {
                    var reduction = GetLateMoveReduction(move, tableMove, depth, searchedMoves, ply, inCheck, alpha, beta);
                    score = -Search(game, depth - 1 - reduction, -alpha - 1, -alpha, ply + 1, true, cancellationToken);
                    if (reduction > 0 && score > alpha)
                    {
                        score = -Search(game, depth - 1, -alpha - 1, -alpha, ply + 1, true, cancellationToken);
                    }

                    if (score > alpha && score < beta)
                    {
                        score = -Search(game, depth - 1, -beta, -alpha, ply + 1, true, cancellationToken);
                    }
                }
            }
            finally
            {
                game.UndoMove();
            }

            searchedMoves++;

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
                if (!move.IsCapture)
                {
                    StoreKiller(ply, move);
                    StoreHistory(move, depth);
                }

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

    private static bool ShouldTryNullMove(Game game, int depth, int alpha, int beta, bool allowNullMove, bool inCheck)
    {
        return allowNullMove
            && depth >= NullMoveMinDepth
            && !inCheck
            && alpha > -MateThreshold
            && beta < MateThreshold
            && HasNonPawnMaterial(game);
    }

    private static bool HasNonPawnMaterial(Game game)
    {
        var position = game.Position;
        var pieces = position.WhiteToMove
            ? position.WhiteKnights | position.WhiteBishops | position.WhiteRooks | position.WhiteQueens
            : position.BlackKnights | position.BlackBishops | position.BlackRooks | position.BlackQueens;

        return pieces.IsNotEmpty();
    }

    private int Quiescence(Game game, int alpha, int beta, int ply, int qPly, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        RecordNode();

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
            var move = PickNextMove(moves, moveCount, i, Move.NullMove, ply, useKillers: false);
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

    private void RecordNode()
    {
        _nodes++;
        if (_nodes >= _nodeLimit)
        {
            throw new NodeLimitReachedException();
        }
    }

    private sealed class NodeLimitReachedException : Exception
    {
    }
}
