using ChessLite.Movement;
using ChessLite.Primitives;

namespace Veloce.Uci;

public static class UciMoveFormatter
{
    public static string Format(Move move)
    {
        var promotion = move.PromotedPieceType switch
        {
            PromotedPieceType.None => string.Empty,
            PromotedPieceType.Knight => "n",
            PromotedPieceType.Bishop => "b",
            PromotedPieceType.Rook => "r",
            PromotedPieceType.Queen => "q",
            _ => throw new ArgumentOutOfRangeException(nameof(move), "Unknown promotion piece."),
        };

        return $"{move.From}{move.To}{promotion}";
    }
}
