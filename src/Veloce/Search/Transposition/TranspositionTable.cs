namespace Veloce.Search.Transposition;

internal sealed class TranspositionTable
{
    private const int EntrySizeBytes = 16;
    private const int DefaultMegabytes = 16;

    private TranspositionEntry[] _entries = [];
    private int _mask;
    private byte _generation;

    public TranspositionTable()
    {
        Resize(DefaultMegabytes);
    }

    public void Resize(int megabytes)
    {
        var bytes = Math.Max(1, megabytes) * 1024L * 1024L;
        var entryCount = PreviousPowerOfTwo(Math.Max(1, bytes / EntrySizeBytes));

        _entries = new TranspositionEntry[entryCount];
        _mask = entryCount - 1;
        _generation = 0;
    }

    public void NewSearch()
    {
        _generation = (byte)((_generation + 1) & 0x3F);
    }

    public bool TryGet(ulong key, out TranspositionEntry entry)
    {
        entry = _entries[(int)key & _mask];
        return entry.Key == key;
    }

    public void Store(ulong key, int score, CompactMove move, int depth, TranspositionBound bound)
    {
        var index = (int)key & _mask;
        var existing = _entries[index];
        if (!ShouldReplace(existing, key, depth))
        {
            return;
        }

        _entries[index] = new TranspositionEntry(key, score, move, depth, bound, _generation);
    }

    private bool ShouldReplace(TranspositionEntry existing, ulong key, int depth)
    {
        return existing.IsEmpty
            || existing.Key == key
            || existing.Generation != _generation
            || depth >= existing.Depth;
    }

    private static int PreviousPowerOfTwo(long value)
    {
        var result = 1;
        while ((long)result << 1 <= value && result <= 1 << 29)
        {
            result <<= 1;
        }

        return result;
    }
}
