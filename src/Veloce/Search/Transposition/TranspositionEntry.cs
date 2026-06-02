namespace Veloce.Search.Transposition;

internal readonly struct TranspositionEntry
{
    private const int MoveOffset = 32;
    private const int DepthOffset = 48;
    private const int BoundOffset = 56;
    private const int GenerationOffset = 58;

    private const ulong ScoreMask = 0xFFFF_FFFFUL;
    private const ulong MoveMask = 0xFFFFUL;
    private const ulong DepthMask = 0xFFUL;
    private const ulong BoundMask = 0x3UL;
    private const ulong GenerationMask = 0x3FUL;

    private readonly ulong _packed;

    public TranspositionEntry(ulong key, int score, CompactMove move, int depth, TranspositionBound bound, byte generation)
    {
        Key = key;
        _packed = ((ulong)(uint)score & ScoreMask)
            | ((ulong)(ushort)move << MoveOffset)
            | ((ulong)(byte)Math.Clamp(depth, 0, byte.MaxValue) << DepthOffset)
            | ((ulong)bound << BoundOffset)
            | ((ulong)(generation & GenerationMask) << GenerationOffset);
    }

    public TranspositionEntry(ulong key, ulong packed)
    {
        Key = key;
        _packed = packed;
    }

    public ulong Key { get; }

    public ulong Packed => _packed;

    public bool IsEmpty => Key == 0;

    public int Score => (int)(uint)(_packed & ScoreMask);

    public CompactMove Move => (ushort)((_packed >> MoveOffset) & MoveMask);

    public int Depth => (int)((_packed >> DepthOffset) & DepthMask);

    public TranspositionBound Bound => (TranspositionBound)((_packed >> BoundOffset) & BoundMask);

    public byte Generation => (byte)((_packed >> GenerationOffset) & GenerationMask);
}
