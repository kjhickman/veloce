using Zugfish.Engine;
using Zugfish.Engine.Models;

namespace Zugfish.UCI.Lib;

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
        var from = AlgebraicFromSquare(move.From);
        var to = AlgebraicFromSquare(move.To);
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

    public static string AlgebraicFromSquare(int square)
    {
        var file = (char)('a' + (square & 7));
        var rank = (char)('1' + (square >> 3));
        return $"{file}{rank}";
    }

    public static Position? ParsePosition(string[] parts)
    {
        if (parts.Length < 2) return null;

        switch (parts[1])
        {
            case "startpos":
            {
                var position = new Position();
                var movesIndex = Array.IndexOf(parts, "moves");
                if (movesIndex != -1)
                {
                    // Apply all moves after "moves"
                    for (var i = movesIndex + 1; i < parts.Length; i++)
                    {
                        position.MakeMove(parts[i]);
                    }
                }

                return position;
            }
            case "fen" when parts.Length >= 7:
            {
                var fen = string.Join(" ", parts.Skip(2).Take(6));
                var position = new Position(fen);

                var movesIndex = Array.IndexOf(parts, "moves");
                if (movesIndex != -1)
                {
                    // Apply all moves after "moves"
                    for (var i = movesIndex + 1; i < parts.Length; i++)
                    {
                        position.MakeMove(parts[i]);
                    }
                }

                return position;
            }
            default:
                return null;
        }
    }
}
