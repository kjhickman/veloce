using Veloce;
using Veloce.Models;

namespace Veloce.Uci.Lib.Extensions;

public static class PositionExtensions
{
    public static PieceType GetPieceTypeAt(this Position position, Square square, bool isWhite)
    {
        var mask = Bitboard.Mask(square);
        if (isWhite)
        {
            if ((position.WhitePawns & mask).IsNotEmpty()) return PieceType.WhitePawn;
            if ((position.WhiteKnights & mask).IsNotEmpty()) return PieceType.WhiteKnight;
            if ((position.WhiteBishops & mask).IsNotEmpty()) return PieceType.WhiteBishop;
            if ((position.WhiteRooks & mask).IsNotEmpty()) return PieceType.WhiteRook;
            if ((position.WhiteQueens & mask).IsNotEmpty()) return PieceType.WhiteQueen;
            if ((position.WhiteKing & mask).IsNotEmpty()) return PieceType.WhiteKing;
        }
        else
        {
            if ((position.BlackPawns & mask).IsNotEmpty()) return PieceType.BlackPawn;
            if ((position.BlackKnights & mask).IsNotEmpty()) return PieceType.BlackKnight;
            if ((position.BlackBishops & mask).IsNotEmpty()) return PieceType.BlackBishop;
            if ((position.BlackRooks & mask).IsNotEmpty()) return PieceType.BlackRook;
            if ((position.BlackQueens & mask).IsNotEmpty()) return PieceType.BlackQueen;
            if ((position.BlackKing & mask).IsNotEmpty()) return PieceType.BlackKing;
        }

        return PieceType.None;
    }
}
