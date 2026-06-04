using ChessLite.Movement;
using ChessLite.Primitives;
using Veloce.Evaluation;
using Veloce.Search.Transposition;

namespace Veloce.Search;

public sealed partial class NegamaxSearch
{
    private Move PickNextRootMove(Span<Move> moves, int moveCount, int startIndex, Move tableMove, Move diversifiedRootMove)
    {
        if (startIndex == 0 && diversifiedRootMove != Move.NullMove)
        {
            for (var i = 0; i < moveCount; i++)
            {
                if (moves[i] == diversifiedRootMove)
                {
                    (moves[startIndex], moves[i]) = (moves[i], moves[startIndex]);
                    return moves[startIndex];
                }
            }
        }

        return PickNextMove(moves, moveCount, startIndex, tableMove, 0, useKillers: false);
    }

    private Move PickNextMove(Span<Move> moves, int moveCount, int startIndex, Move tableMove, int ply, bool useKillers)
    {
        var bestIndex = startIndex;
        var bestScore = ScoreMove(moves[startIndex], tableMove, ply, useKillers);

        for (var i = startIndex + 1; i < moveCount; i++)
        {
            var score = ScoreMove(moves[i], tableMove, ply, useKillers);
            if (score > bestScore)
            {
                bestScore = score;
                bestIndex = i;
            }
        }

        (moves[startIndex], moves[bestIndex]) = (moves[bestIndex], moves[startIndex]);
        return moves[startIndex];
    }

    private int ScoreMove(Move move, Move tableMove, int ply, bool useKillers)
    {
        if (move == tableMove)
        {
            return TableMoveScore;
        }

        if (move.IsCapture)
        {
            return CaptureMoveScore + GetPieceValue(move.CapturedPieceType) * 16 - GetPieceValue(move.PieceType);
        }

        if (!useKillers || ply >= MaxSearchPly)
        {
            return 0;
        }

        var compactMove = new CompactMove(move);
        if (compactMove == _primaryKillers[ply])
        {
            return PrimaryKillerScore;
        }

        if (compactMove == _secondaryKillers[ply])
        {
            return SecondaryKillerScore;
        }

        return _history[GetSideIndex(move.PieceType), (int)move.From, (int)move.To];
    }

    private int GetLateMoveReduction(Move move, Move tableMove, int depth, int searchedMoves, int ply, bool inCheck, int alpha, int beta)
    {
        return depth >= LateMoveReductionMinDepth
            && searchedMoves >= LateMoveReductionMoveNumber
            && !inCheck
            && !move.IsCapture
            && move != tableMove
            && !IsKillerMove(ply, move)
            && alpha > -MateThreshold
            && beta < MateThreshold
            ? LateMoveReduction
            : 0;
    }

    private bool IsKillerMove(int ply, Move move)
    {
        if (ply >= MaxSearchPly)
        {
            return false;
        }

        var compactMove = new CompactMove(move);
        return compactMove == _primaryKillers[ply] || compactMove == _secondaryKillers[ply];
    }

    private void StoreKiller(int ply, Move move)
    {
        if (ply >= MaxSearchPly)
        {
            return;
        }

        var compactMove = new CompactMove(move);
        if (compactMove == _primaryKillers[ply])
        {
            return;
        }

        _secondaryKillers[ply] = _primaryKillers[ply];
        _primaryKillers[ply] = compactMove;
    }

    private void StoreHistory(Move move, int depth)
    {
        var side = GetSideIndex(move.PieceType);
        var from = (int)move.From;
        var to = (int)move.To;
        var bonus = depth * depth;
        _history[side, from, to] = Math.Min(MaxHistoryScore, _history[side, from, to] + bonus);
    }

    private static int GetSideIndex(PieceType pieceType)
    {
        return pieceType switch
        {
            PieceType.WhitePawn or PieceType.WhiteKnight or PieceType.WhiteBishop or PieceType.WhiteRook or PieceType.WhiteQueen or PieceType.WhiteKing => 0,
            _ => 1,
        };
    }

    private static int GetPieceValue(PieceType pieceType)
    {
        return pieceType switch
        {
            PieceType.WhitePawn or PieceType.BlackPawn => MaterialEvaluator.PawnValue,
            PieceType.WhiteKnight or PieceType.BlackKnight => MaterialEvaluator.KnightValue,
            PieceType.WhiteBishop or PieceType.BlackBishop => MaterialEvaluator.BishopValue,
            PieceType.WhiteRook or PieceType.BlackRook => MaterialEvaluator.RookValue,
            PieceType.WhiteQueen or PieceType.BlackQueen => MaterialEvaluator.QueenValue,
            PieceType.WhiteKing or PieceType.BlackKing => 20_000,
            _ => 0,
        };
    }
}
