using System.Text;
using Zugfish.Engine;

namespace Zugfish.SelfPlay;

public static class BoardRenderer
{
    public static string Render(this Board board)
    {
        var boardArray = new char[64];

        // Fill with empty squares.
        for (var i = 0; i < 64; i++)
            boardArray[i] = '.';

        // Place white pieces.
        PlacePieces(board.WhitePawns, 'P');
        PlacePieces(board.WhiteKnights, 'N');
        PlacePieces(board.WhiteBishops, 'B');
        PlacePieces(board.WhiteRooks, 'R');
        PlacePieces(board.WhiteQueens, 'Q');
        PlacePieces(board.WhiteKing, 'K');

        // Place black pieces.
        PlacePieces(board.BlackPawns, 'p');
        PlacePieces(board.BlackKnights, 'n');
        PlacePieces(board.BlackBishops, 'b');
        PlacePieces(board.BlackRooks, 'r');
        PlacePieces(board.BlackQueens, 'q');
        PlacePieces(board.BlackKing, 'k');

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
