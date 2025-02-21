using Zugfish.Engine;
using Zugfish.Engine.Models;
using static Zugfish.UCI.Lib.Helpers;

namespace Zugfish.UCI.Lib;

public static class BoardExtensions
{
    public static void MakeMove(this Position position, ReadOnlySpan<char> uciMove)
    {
        if (uciMove.Length is < 4 or > 5)
            throw new ArgumentException("Invalid UCI move format.", nameof(uciMove));

        var from = SquareFromUci(uciMove[..2]);
        var to = SquareFromUci(uciMove[2..4]);

        if ((from | to) >> 6 != 0) // Ensure indices are valid
            throw new ArgumentOutOfRangeException(nameof(uciMove), "Square index out of range.");

        var specialMoveType = SpecialMoveType.None;

        var isPawnMove = (position.WhitePawns & (1UL << from)) != 0 || (position.BlackPawns & (1UL << from)) != 0;

        // Detect castling (if king moves two squares)
        if ((from == 4 && to is 6) || (from == 60 && to is 62))
        {
            specialMoveType = SpecialMoveType.ShortCastle;
        }
        else if ((from == 4 && to is 2) || from == 60 && to is 58)
        {
            specialMoveType = SpecialMoveType.LongCastle;
        }
        else if (isPawnMove && (int)position.EnPassantTarget == to) // Detect En Passant
        {
            specialMoveType = SpecialMoveType.EnPassant;
        }
        else if (isPawnMove && Math.Abs(from - to) == 16) // Detect double pawn move
        {
            specialMoveType = SpecialMoveType.DoublePawnPush;
        }

        var isCapture = (Bitboard.Mask(to) & position.AllPieces) != 0 || specialMoveType == SpecialMoveType.EnPassant;
        var capturedPieceType = PieceType.None;
        if (specialMoveType == SpecialMoveType.EnPassant)
        {
            capturedPieceType = position.WhiteToMove ? PieceType.BlackPawn : PieceType.WhitePawn;
        }
        else if (isCapture)
        {
            capturedPieceType = position.GetPieceTypeAt((Square)to, !position.WhiteToMove);
        }

        var promotedPieceType = PromotedPieceType.None;
        if (uciMove.Length == 5) // If the move has a 5th character (promotion), determine its type
        {
            promotedPieceType = uciMove[4] switch
            {
                'q' => PromotedPieceType.Queen,
                'r' => PromotedPieceType.Rook,
                'b' => PromotedPieceType.Bishop,
                'n' => PromotedPieceType.Knight,
                _ => PromotedPieceType.None
            };
        }

        var move = new Move((Square)from, (Square)to, promotedPieceType, position.GetPieceTypeAt((Square)from, position.WhiteToMove), capturedPieceType, isCapture, specialMoveType);
        position.MakeMove(move);
    }
}
