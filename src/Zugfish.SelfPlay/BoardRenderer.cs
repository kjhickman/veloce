using System.Text;
using Zugfish.Engine;
using Zugfish.Engine.Models;

namespace Zugfish.SelfPlay;

public static class BoardRenderer
{
    public static string Render(this Position position)
    {
        var boardArray = new char[64];

        // Fill with empty squares.
        for (var i = 0; i < 64; i++)
            boardArray[i] = '.';

        // Place white pieces.
        PlacePieces(position.WhitePawns, 'P');
        PlacePieces(position.WhiteKnights, 'N');
        PlacePieces(position.WhiteBishops, 'B');
        PlacePieces(position.WhiteRooks, 'R');
        PlacePieces(position.WhiteQueens, 'Q');
        PlacePieces(position.WhiteKing, 'K');

        // Place black pieces.
        PlacePieces(position.BlackPawns, 'p');
        PlacePieces(position.BlackKnights, 'n');
        PlacePieces(position.BlackBishops, 'b');
        PlacePieces(position.BlackRooks, 'r');
        PlacePieces(position.BlackQueens, 'q');
        PlacePieces(position.BlackKing, 'k');

        // Build board string.
        var sb = new StringBuilder();
        sb.AppendLine("  a b c d e f g h");
        sb.AppendLine("  ----------------");
        for (var rank = 7; rank >= 0; rank--)  // Top-down rendering.
        {
            sb.Append($"{rank + 1}| ");
            for (var file = 0; file < 8; file++)
            {
                sb.Append(boardArray[rank * 8 + file]);
                sb.Append(' ');
            }
            sb.AppendLine("|");
        }
        sb.AppendLine("  ----------------");
        return sb.ToString();

        // Local function to place pieces based on bitboard.
        void PlacePieces(Bitboard bitboard, char pieceChar)
        {
            var value = bitboard.Value;
            for (var i = 0; i < 64; i++)
            {
                if ((value & (1UL << i)) != 0)
                    boardArray[i] = pieceChar;
            }
        }
    }
}
