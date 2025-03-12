using Zugfish.Engine;
using Zugfish.Engine.Extensions;
using Zugfish.Engine.Models;
using Zugfish.Uci.Lib.Extensions;

namespace Zugfish.Uci.Lib;

public static class Helpers
{
    public static string UciFromMove(Move move)
    {
        var from = move.From.ToString();
        var to = move.To.ToString();
        var promotion = move.PromotedPieceType switch
        {
            PromotedPieceType.Queen => "q",
            PromotedPieceType.Rook => "r",
            PromotedPieceType.Bishop => "b",
            PromotedPieceType.Knight => "n",
            _ => string.Empty
        };
        return $"{from}{to}{promotion}";
    }

    public static Move MoveFromUci(Position position, ReadOnlySpan<char> uciMove)
    {
        if (uciMove.Length is < 4 or > 5)
            throw new ArgumentException("Invalid UCI move format.", nameof(uciMove));

        var from = SquareFromUci(uciMove[..2]);
        var to = SquareFromUci(uciMove[2..4]);

        if (!from.IsValid() || !to.IsValid()) // Ensure indices are valid
            throw new ArgumentOutOfRangeException(nameof(uciMove), "Square index out of range.");

        var fromMask = from.ToMask();
        var toMask = to.ToMask();

        var specialMoveType = SpecialMoveType.None;

        var isPawnMove = (position.WhitePawns | position.BlackPawns).Intersects(fromMask);
        var isKingMove = (position.WhiteKing | position.BlackKing).Intersects(fromMask);
        var fromRank = from.GetRank();
        var toRank = to.GetRank();

        // Detect castling (if king moves two squares)
        if (isKingMove && fromRank == toRank)
        {
            if (Math.Abs(from - to) == 2)
            {
                specialMoveType = from < to ? SpecialMoveType.ShortCastle : SpecialMoveType.LongCastle;
            }
        }
        else if (isPawnMove && position.EnPassantTarget == to) // Detect En Passant
        {
            specialMoveType = SpecialMoveType.EnPassant;
        }
        else if (isPawnMove && Math.Abs(from - to) == 16) // Detect double pawn move
        {
            specialMoveType = SpecialMoveType.DoublePawnPush;
        }

        var isCapture = (toMask & position.AllPieces).IsNotEmpty() || specialMoveType == SpecialMoveType.EnPassant;
        var capturedPieceType = PieceType.None;
        if (specialMoveType == SpecialMoveType.EnPassant)
        {
            capturedPieceType = position.WhiteToMove ? PieceType.BlackPawn : PieceType.WhitePawn;
        }
        else if (isCapture)
        {
            capturedPieceType = position.GetPieceTypeAt(to, !position.WhiteToMove);
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

        return new Move(from, to, promotedPieceType, position.GetPieceTypeAt(from, position.WhiteToMove), capturedPieceType, isCapture, specialMoveType);
    }

    private static Square SquareFromUci(ReadOnlySpan<char> square)
    {
        if (square.Length != 2)
            throw new ArgumentException("Invalid square length", nameof(square));

        var file = square[0];
        var rank = square[1];

        if (file < 'a' || file > 'h' || rank < '1' || rank > '8')
            throw new ArgumentException("Invalid UCI square.", nameof(file));

        return (Square)((rank - '1') * 8 + (file - 'a'));
    }
}
