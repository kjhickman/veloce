namespace Veloce.Search.Transposition;

internal sealed class TranspositionTable
{
    private const int EntrySizeBytes = 16;
    private const int DefaultMegabytes = 16;
    private const int MaxEntryCount = 1 << 30;

    private long[] _keys = [];
    private long[] _packedEntries = [];
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

        _keys = new long[entryCount];
        _packedEntries = new long[entryCount];
        _mask = entryCount - 1;
        _generation = 0;
    }

    public void NewSearch()
    {
        _generation = (byte)((_generation + 1) & 0x3F);
    }

    public bool TryGet(ulong key, out TranspositionEntry entry)
    {
        var index = (int)key & _mask;
        var storedKey = (ulong)Volatile.Read(ref _keys[index]);
        if (storedKey != key)
        {
            entry = default;
            return false;
        }

        var packed = (ulong)Volatile.Read(ref _packedEntries[index]);
        if ((ulong)Volatile.Read(ref _keys[index]) != key)
        {
            entry = default;
            return false;
        }

        entry = new TranspositionEntry(key, packed);
        return true;
    }

    public void Store(ulong key, int score, CompactMove move, int depth, TranspositionBound bound)
    {
        var index = (int)key & _mask;
        var storedKey = (ulong)Volatile.Read(ref _keys[index]);
        var existing = storedKey == 0
            ? default
            : new TranspositionEntry(storedKey, (ulong)Volatile.Read(ref _packedEntries[index]));
        if (!ShouldReplace(existing, key, depth))
        {
            return;
        }

        var entry = new TranspositionEntry(key, score, move, depth, bound, _generation);
        Volatile.Write(ref _packedEntries[index], (long)entry.Packed);
        Volatile.Write(ref _keys[index], (long)key);
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
        while ((long)result << 1 <= value && result < MaxEntryCount)
        {
            result <<= 1;
        }

        return result;
    }
}
