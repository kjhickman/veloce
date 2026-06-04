namespace Veloce.Search.Transposition;

internal sealed class TranspositionTable
{
    private const int EntriesPerBucket = 2;
    private const int EntrySizeBytes = 16;
    private const int BucketSizeBytes = EntriesPerBucket * EntrySizeBytes;
    private const int DefaultMegabytes = 16;
    private const int MaxBucketCount = 1 << 29;

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
        var bucketCount = PreviousPowerOfTwo(Math.Max(1, bytes / BucketSizeBytes));
        var entryCount = bucketCount * EntriesPerBucket;

        _keys = new long[entryCount];
        _packedEntries = new long[entryCount];
        _mask = bucketCount - 1;
        _generation = 0;
    }

    public void Clear()
    {
        Array.Clear(_keys);
        Array.Clear(_packedEntries);
        _generation = 0;
    }

    public int HashFull
    {
        get
        {
            var sampleCount = Math.Min(1000, _keys.Length);
            if (sampleCount == 0)
            {
                return 0;
            }

            var used = 0;
            for (var i = 0; i < sampleCount; i++)
            {
                if (Volatile.Read(ref _keys[i]) != 0)
                {
                    used++;
                }
            }

            return used * 1000 / sampleCount;
        }
    }

    public void NewSearch()
    {
        _generation = (byte)((_generation + 1) & 0x3F);
    }

    public bool TryGet(ulong key, out TranspositionEntry entry)
    {
        var startIndex = GetBucketStartIndex(key);
        for (var i = 0; i < EntriesPerBucket; i++)
        {
            var index = startIndex + i;
            var storedKey = (ulong)Volatile.Read(ref _keys[index]);
            if (storedKey != key)
            {
                continue;
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

        entry = default;
        return false;
    }

    public void Store(ulong key, int score, CompactMove move, int depth, TranspositionBound bound)
    {
        var index = FindReplacementIndex(key, depth);
        if (index < 0)
        {
            return;
        }

        var entry = new TranspositionEntry(key, score, move, depth, bound, _generation);
        Volatile.Write(ref _packedEntries[index], (long)entry.Packed);
        Volatile.Write(ref _keys[index], (long)key);
    }

    private int FindReplacementIndex(ulong key, int depth)
    {
        var startIndex = GetBucketStartIndex(key);

        for (var i = 0; i < EntriesPerBucket; i++)
        {
            var index = startIndex + i;
            var storedKey = (ulong)Volatile.Read(ref _keys[index]);
            if (storedKey == key)
            {
                return index;
            }
        }

        for (var i = 0; i < EntriesPerBucket; i++)
        {
            var index = startIndex + i;
            if (Volatile.Read(ref _keys[index]) == 0)
            {
                return index;
            }
        }

        for (var i = 0; i < EntriesPerBucket; i++)
        {
            var index = startIndex + i;
            var storedKey = (ulong)Volatile.Read(ref _keys[index]);
            var existing = new TranspositionEntry(storedKey, (ulong)Volatile.Read(ref _packedEntries[index]));
            if (existing.Generation != _generation)
            {
                return index;
            }
        }

        var replaceIndex = startIndex;
        var replacePriority = int.MaxValue;

        for (var i = 0; i < EntriesPerBucket; i++)
        {
            var index = startIndex + i;
            var storedKey = (ulong)Volatile.Read(ref _keys[index]);
            var existing = new TranspositionEntry(storedKey, (ulong)Volatile.Read(ref _packedEntries[index]));

            var priority = existing.Depth - (RelativeAge(existing.Generation) * 8);
            if (priority < replacePriority)
            {
                replacePriority = priority;
                replaceIndex = index;
            }
        }

        return depth >= replacePriority ? replaceIndex : -1;
    }

    private int GetBucketStartIndex(ulong key)
    {
        return ((int)key & _mask) * EntriesPerBucket;
    }

    private int RelativeAge(byte generation)
    {
        return (_generation - generation) & 0x3F;
    }

    private static int PreviousPowerOfTwo(long value)
    {
        var result = 1;
        while ((long)result << 1 <= value && result < MaxBucketCount)
        {
            result <<= 1;
        }

        return result;
    }
}
