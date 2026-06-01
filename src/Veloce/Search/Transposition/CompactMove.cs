using ChessLite.Movement;
using ChessLite.Primitives;

namespace Veloce.Search.Transposition;

public readonly struct CompactMove : IEquatable<CompactMove>
{
    private readonly ushort _packed;

    public CompactMove(ushort packed)
    {
        _packed = packed;
    }

    public CompactMove(Move move)
    {
        if (move == Move.NullMove)
        {
            _packed = 0;
            return;
        }

        var compact = (ushort)((int)move.From | ((int)move.To << 6));
        if (move.PromotedPieceType != PromotedPieceType.None)
        {
            compact |= (ushort)((int)move.PromotedPieceType << 12);
        }

        _packed = compact;
    }

    public static implicit operator CompactMove(ushort value) => new(value);

    public static implicit operator ushort(CompactMove value) => value._packed;

    public static bool operator ==(CompactMove left, CompactMove right) => left.Equals(right);

    public static bool operator !=(CompactMove left, CompactMove right) => !(left == right);

    public Move FindMatchingMove(ReadOnlySpan<Move> moveList, int moveCount)
    {
        if (_packed == 0) return Move.NullMove;

        var from = (Square)(_packed & 0x3F);
        var to = (Square)((_packed >> 6) & 0x3F);
        var promotedType = (PromotedPieceType)((_packed >> 12) & 0xF);

        for (var i = 0; i < moveCount; i++)
        {
            var move = moveList[i];
            if (move.From != from || move.To != to)
            {
                continue;
            }

            if (promotedType == PromotedPieceType.None || move.PromotedPieceType == promotedType)
            {
                return move;
            }
        }

        return Move.NullMove;
    }

    public bool Equals(CompactMove other) => _packed == other._packed;

    public override bool Equals(object? obj) => obj is CompactMove other && Equals(other);

    public override int GetHashCode() => _packed;
}
