namespace Zugfish.Engine.Models;

public struct MoveHistory
{
    public Bitboard? CapturedPiece; // Bitboard with only the captured piece
    public Bitboard FromSquare;    // Bitboard with only the moved piece at its original position
    public Bitboard ToSquare;      // Bitboard with only the moved piece at its new position
    public Move Move;              // The move that was made
    public PieceType CapturedPieceType;
    public PieceType MovedPieceType;
    public CastlingRights PreviousCastlingRights;
    public Square PreviousEnPassantTarget;
    public int PreviousHalfmoveClock;
    public ulong PreviousZobristHash;
    public Bitboard PreviousWhiteAttacks;
    public Bitboard PreviousBlackAttacks;
}
