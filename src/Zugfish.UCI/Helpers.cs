using Zugfish.Engine;

namespace Zugfish.UCI;

public class Helpers
{
    /// <summary>
    /// Converts a two-character UCI square (e.g. "e4") to its square index.
    /// </summary>
    public static int SquareFromUci(ReadOnlySpan<char> square)
    {
        if (square.Length != 2)
            throw new ArgumentException("Invalid square length", nameof(square));

        var file = square[0];
        var rank = square[1];

        if (file < 'a' || file > 'h' || rank < '1' || rank > '8')
            throw new ArgumentException("Invalid UCI square.", nameof(file));

        return (rank - '1') * 8 + (file - 'a');
    }

    public static string UciFromMove(Move move)
    {
        var from = UciFromSquare(move.From);
        var to = UciFromSquare(move.To);
        var promotion = move.Type switch
        {
            MoveType.PromoteToQueen => "q",
            MoveType.PromoteToRook => "r",
            MoveType.PromoteToBishop => "b",
            MoveType.PromoteToKnight => "n",
            _ => ""
        };
        return $"{from}{to}{promotion}";
    }

    public static string UciFromSquare(int square)
    {
        var file = (char)('a' + (square & 7));
        var rank = (char)('1' + (square >> 3));
        return $"{file}{rank}";
    }
}
