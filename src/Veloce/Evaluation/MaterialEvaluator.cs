using ChessLite.Primitives;
using ChessLite.State;

namespace Veloce.Evaluation;

public static class MaterialEvaluator
{
    internal const int PawnValue = 100;
    internal const int KnightValue = 320;
    internal const int BishopValue = 330;
    internal const int RookValue = 500;
    internal const int QueenValue = 900;
    private const int MaxGamePhase = 24;

    private static readonly int[] MiddleGamePawnTable =
    [
         0,  0,  0,  0,  0,  0,  0,  0,
         5, 10, 10,-20,-20, 10, 10,  5,
         5, -5,-10,  0,  0,-10, -5,  5,
         0,  0,  0, 20, 20,  0,  0,  0,
         5,  5, 10, 25, 25, 10,  5,  5,
        10, 10, 20, 30, 30, 20, 10, 10,
        50, 50, 50, 50, 50, 50, 50, 50,
         0,  0,  0,  0,  0,  0,  0,  0,
    ];

    private static readonly int[] EndGamePawnTable =
    [
          0,   0,   0,   0,   0,   0,   0,   0,
          0,   0,   0, -10, -10,   0,   0,   0,
          5,   5,   5,  10,  10,   5,   5,   5,
         10,  10,  15,  25,  25,  15,  10,  10,
         20,  20,  25,  35,  35,  25,  20,  20,
         40,  40,  45,  50,  50,  45,  40,  40,
         80,  80,  80,  80,  80,  80,  80,  80,
          0,   0,   0,   0,   0,   0,   0,   0,
    ];

    private static readonly int[] MiddleGameKnightTable =
    [
        -50,-40,-30,-30,-30,-30,-40,-50,
        -40,-20,  0,  0,  0,  0,-20,-40,
        -30,  0, 10, 15, 15, 10,  0,-30,
        -30,  5, 15, 20, 20, 15,  5,-30,
        -30,  0, 15, 20, 20, 15,  0,-30,
        -30,  5, 10, 15, 15, 10,  5,-30,
        -40,-20,  0,  5,  5,  0,-20,-40,
        -50,-40,-30,-30,-30,-30,-40,-50,
    ];

    private static readonly int[] EndGameKnightTable =
    [
        -50,-40,-30,-30,-30,-30,-40,-50,
        -40,-20,  0,  5,  5,  0,-20,-40,
        -30,  0, 10, 15, 15, 10,  0,-30,
        -30,  5, 15, 20, 20, 15,  5,-30,
        -30,  0, 15, 20, 20, 15,  0,-30,
        -30,  5, 10, 15, 15, 10,  5,-30,
        -40,-20,  0,  0,  0,  0,-20,-40,
        -50,-40,-30,-30,-30,-30,-40,-50,
    ];

    private static readonly int[] MiddleGameBishopTable =
    [
        -20,-10,-10,-10,-10,-10,-10,-20,
        -10,  5,  0,  0,  0,  0,  5,-10,
        -10, 10, 10, 10, 10, 10, 10,-10,
        -10,  0, 10, 10, 10, 10,  0,-10,
        -10,  5,  5, 10, 10,  5,  5,-10,
        -10,  0,  5, 10, 10,  5,  0,-10,
        -10,  0,  0,  0,  0,  0,  0,-10,
        -20,-10,-10,-10,-10,-10,-10,-20,
    ];

    private static readonly int[] EndGameBishopTable =
    [
        -20,-10,-10,-10,-10,-10,-10,-20,
        -10,  5,  0,  0,  0,  0,  5,-10,
        -10, 10, 10, 10, 10, 10, 10,-10,
        -10,  0, 10, 10, 10, 10,  0,-10,
        -10,  5,  5, 10, 10,  5,  5,-10,
        -10,  0,  5, 10, 10,  5,  0,-10,
        -10,  0,  0,  0,  0,  0,  0,-10,
        -20,-10,-10,-10,-10,-10,-10,-20,
    ];

    private static readonly int[] MiddleGameRookTable =
    [
         0,  0,  0,  5,  5,  0,  0,  0,
         -5,  0,  0,  0,  0,  0,  0, -5,
         -5,  0,  0,  0,  0,  0,  0, -5,
         -5,  0,  0,  0,  0,  0,  0, -5,
         -5,  0,  0,  0,  0,  0,  0, -5,
         -5,  0,  0,  0,  0,  0,  0, -5,
          5, 10, 10, 10, 10, 10, 10,  5,
          0,  0,  0,  0,  0,  0,  0,  0,
    ];

    private static readonly int[] EndGameRookTable =
    [
        -10, -5,  0,  0,  0,  0, -5,-10,
          0,  0,  5,  5,  5,  5,  0,  0,
          0,  5, 10, 10, 10, 10,  5,  0,
          0,  5, 10, 10, 10, 10,  5,  0,
          0,  5, 10, 10, 10, 10,  5,  0,
          0,  5, 10, 10, 10, 10,  5,  0,
         10, 15, 15, 15, 15, 15, 15, 10,
          0,  5,  5, 10, 10,  5,  5,  0,
    ];

    private static readonly int[] MiddleGameQueenTable =
    [
        -20,-10,-10, -5, -5,-10,-10,-20,
        -10,  0,  5,  0,  0,  0,  0,-10,
        -10,  5,  5,  5,  5,  5,  0,-10,
          0,  0,  5,  5,  5,  5,  0, -5,
         -5,  0,  5,  5,  5,  5,  0, -5,
        -10,  0,  5,  5,  5,  5,  0,-10,
        -10,  0,  0,  0,  0,  0,  0,-10,
        -20,-10,-10, -5, -5,-10,-10,-20,
    ];

    private static readonly int[] EndGameQueenTable =
    [
        -20,-10,-10, -5, -5,-10,-10,-20,
        -10,  0,  5,  0,  0,  0,  0,-10,
        -10,  5,  5,  5,  5,  5,  0,-10,
          0,  0,  5,  5,  5,  5,  0, -5,
         -5,  0,  5,  5,  5,  5,  0, -5,
        -10,  0,  5,  5,  5,  5,  0,-10,
        -10,  0,  0,  0,  0,  0,  0,-10,
        -20,-10,-10, -5, -5,-10,-10,-20,
    ];

    private static readonly int[] MiddleGameKingTable =
    [
         20, 30, 10,  0,  0, 10, 30, 20,
         20, 20,  0,  0,  0,  0, 20, 20,
        -10,-20,-20,-20,-20,-20,-20,-10,
        -20,-30,-30,-40,-40,-30,-30,-20,
        -30,-40,-40,-50,-50,-40,-40,-30,
        -30,-40,-40,-50,-50,-40,-40,-30,
        -30,-40,-40,-50,-50,-40,-40,-30,
        -30,-40,-40,-50,-50,-40,-40,-30,
    ];

    private static readonly int[] EndGameKingTable =
    [
        -50,-30,-20,-10,-10,-20,-30,-50,
        -30,-10,  0, 10, 10,  0,-10,-30,
        -20,  0, 20, 30, 30, 20,  0,-20,
        -10, 10, 30, 40, 40, 30, 10,-10,
        -10, 10, 30, 40, 40, 30, 10,-10,
        -20,  0, 20, 30, 30, 20,  0,-20,
        -30,-10,  0, 10, 10,  0,-10,-30,
        -50,-30,-20,-10,-10,-20,-30,-50,
    ];

    public static int Evaluate(this Position position)
    {
        var score = EvaluateMaterial(position) + EvaluatePosition(position);
        return position.WhiteToMove ? score : -score;
    }

    private static int EvaluateMaterial(Position position)
    {
        var materialScore = PawnValue * position.WhitePawns.Count() - PawnValue * position.BlackPawns.Count();
        materialScore += KnightValue * position.WhiteKnights.Count() - KnightValue * position.BlackKnights.Count();
        materialScore += BishopValue * position.WhiteBishops.Count() - BishopValue * position.BlackBishops.Count();
        materialScore += RookValue * position.WhiteRooks.Count() - RookValue * position.BlackRooks.Count();
        materialScore += QueenValue * position.WhiteQueens.Count() - QueenValue * position.BlackQueens.Count();

        return materialScore;
    }

    private static int EvaluatePosition(Position position)
    {
        var middleGameScore = 0;
        var endGameScore = 0;

        AddPiecePositions(position.WhitePawns, MiddleGamePawnTable, EndGamePawnTable, true, ref middleGameScore, ref endGameScore);
        AddPiecePositions(position.WhiteKnights, MiddleGameKnightTable, EndGameKnightTable, true, ref middleGameScore, ref endGameScore);
        AddPiecePositions(position.WhiteBishops, MiddleGameBishopTable, EndGameBishopTable, true, ref middleGameScore, ref endGameScore);
        AddPiecePositions(position.WhiteRooks, MiddleGameRookTable, EndGameRookTable, true, ref middleGameScore, ref endGameScore);
        AddPiecePositions(position.WhiteQueens, MiddleGameQueenTable, EndGameQueenTable, true, ref middleGameScore, ref endGameScore);
        AddPiecePositions(position.WhiteKing, MiddleGameKingTable, EndGameKingTable, true, ref middleGameScore, ref endGameScore);

        AddPiecePositions(position.BlackPawns, MiddleGamePawnTable, EndGamePawnTable, false, ref middleGameScore, ref endGameScore);
        AddPiecePositions(position.BlackKnights, MiddleGameKnightTable, EndGameKnightTable, false, ref middleGameScore, ref endGameScore);
        AddPiecePositions(position.BlackBishops, MiddleGameBishopTable, EndGameBishopTable, false, ref middleGameScore, ref endGameScore);
        AddPiecePositions(position.BlackRooks, MiddleGameRookTable, EndGameRookTable, false, ref middleGameScore, ref endGameScore);
        AddPiecePositions(position.BlackQueens, MiddleGameQueenTable, EndGameQueenTable, false, ref middleGameScore, ref endGameScore);
        AddPiecePositions(position.BlackKing, MiddleGameKingTable, EndGameKingTable, false, ref middleGameScore, ref endGameScore);

        var phase = EvaluateGamePhase(position);
        return (middleGameScore * phase + endGameScore * (MaxGamePhase - phase)) / MaxGamePhase;
    }

    private static void AddPiecePositions(
        Bitboard pieces,
        int[] middleGameTable,
        int[] endGameTable,
        bool isWhite,
        ref int middleGameScore,
        ref int endGameScore)
    {
        var remainingPieces = pieces;
        while (remainingPieces.IsNotEmpty())
        {
            var square = remainingPieces.GetFirstSquare();
            var index = isWhite ? (int)square : 63 - (int)square;
            var sign = isWhite ? 1 : -1;

            middleGameScore += sign * middleGameTable[index];
            endGameScore += sign * endGameTable[index];

            remainingPieces &= remainingPieces - 1;
        }
    }

    private static int EvaluateGamePhase(Position position)
    {
        var phase = 0;
        phase += position.WhiteKnights.Count() + position.BlackKnights.Count();
        phase += position.WhiteBishops.Count() + position.BlackBishops.Count();
        phase += 2 * (position.WhiteRooks.Count() + position.BlackRooks.Count());
        phase += 4 * (position.WhiteQueens.Count() + position.BlackQueens.Count());

        return Math.Min(phase, MaxGamePhase);
    }
}
