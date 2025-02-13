using Zugfish.Engine;
using static Zugfish.UCI.Helpers;

namespace Zugfish.UCI;

public static class BoardExtensions
{
    public static void MakeMove(this Board board, ReadOnlySpan<char> uciMove)
    {
        if (uciMove.Length is < 4 or > 5)
            throw new ArgumentException("Invalid UCI move format.", nameof(uciMove));

        var from = SquareFromUci(uciMove[..2]);
        var to = SquareFromUci(uciMove[2..4]);

        if ((from | to) >> 6 != 0) // Ensure indices are valid
            throw new ArgumentOutOfRangeException(nameof(uciMove), "Square index out of range.");

        // Default to a quiet move
        var moveType = MoveType.Quiet;

        var isPawnMove = (board.WhitePawns & (1UL << from)) != 0 || (board.BlackPawns & (1UL << from)) != 0;

        // Detect castling (if king moves two squares)
        if ((from == 4 && to is 2 or 6) || (from == 60 && to is 58 or 62))
        {
            moveType = MoveType.Castling;
        }
        else if (uciMove.Length == 5) // If the move has a 5th character (promotion), determine its type
        {
            moveType = uciMove[4] switch
            {
                'q' => MoveType.PromoteToQueen,
                'r' => MoveType.PromoteToRook,
                'b' => MoveType.PromoteToBishop,
                'n' => MoveType.PromoteToKnight,
                _ => throw new ArgumentException("Invalid promotion piece.", nameof(uciMove))
            };
        }
        else if (isPawnMove && board.EnPassantTarget == to) // Detect En Passant
        {
            moveType = MoveType.EnPassant;
        }
        else if (isPawnMove && Math.Abs(from - to) == 16) // Detect double pawn move
        {
            moveType = MoveType.DoublePawnPush;
        }
        else if ((Bitboard.Mask(to) & board.AllPieces) != 0) // Detect capture
        {
            moveType = MoveType.Capture;
        }

        var move = new Move(from, to, moveType);
        board.MakeMove(move);
    }
}
