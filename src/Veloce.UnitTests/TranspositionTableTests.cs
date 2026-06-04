using Veloce.Search.Transposition;

namespace Veloce.UnitTests;

public class TranspositionTableTests
{
    private const int OneMegabyteBucketCount = 32_768;

    [Test]
    public async Task TryGet_WhenTwoKeysShareBucket_ReturnsBothEntries()
    {
        var table = new TranspositionTable();
        table.Resize(1);
        var firstKey = 1UL;
        var secondKey = firstKey + OneMegabyteBucketCount;

        table.Store(firstKey, 10, 0, 4, TranspositionBound.Exact);
        table.Store(secondKey, 20, 0, 5, TranspositionBound.Lower);

        var foundFirst = table.TryGet(firstKey, out var firstEntry);
        var foundSecond = table.TryGet(secondKey, out var secondEntry);

        await Assert.That(foundFirst).IsTrue();
        await Assert.That(firstEntry.Score).IsEqualTo(10);
        await Assert.That(firstEntry.Depth).IsEqualTo(4);
        await Assert.That(firstEntry.Bound).IsEqualTo(TranspositionBound.Exact);
        await Assert.That(foundSecond).IsTrue();
        await Assert.That(secondEntry.Score).IsEqualTo(20);
        await Assert.That(secondEntry.Depth).IsEqualTo(5);
        await Assert.That(secondEntry.Bound).IsEqualTo(TranspositionBound.Lower);
    }

    [Test]
    public async Task Store_WhenBucketIsFull_DoesNotReplaceDeeperCurrentEntriesWithShallowEntry()
    {
        var table = new TranspositionTable();
        table.Resize(1);
        var firstKey = 1UL;
        var secondKey = firstKey + OneMegabyteBucketCount;
        var shallowKey = firstKey + (2UL * OneMegabyteBucketCount);

        table.Store(firstKey, 10, 0, 6, TranspositionBound.Exact);
        table.Store(secondKey, 20, 0, 7, TranspositionBound.Exact);
        table.Store(shallowKey, 30, 0, 3, TranspositionBound.Exact);

        await Assert.That(table.TryGet(firstKey, out _)).IsTrue();
        await Assert.That(table.TryGet(secondKey, out _)).IsTrue();
        await Assert.That(table.TryGet(shallowKey, out _)).IsFalse();
    }

    [Test]
    public async Task Store_WhenBucketIsFull_ReplacesLowestDepthCurrentEntryWithDeeperEntry()
    {
        var table = new TranspositionTable();
        table.Resize(1);
        var firstKey = 1UL;
        var secondKey = firstKey + OneMegabyteBucketCount;
        var deeperKey = firstKey + (2UL * OneMegabyteBucketCount);

        table.Store(firstKey, 10, 0, 4, TranspositionBound.Exact);
        table.Store(secondKey, 20, 0, 6, TranspositionBound.Exact);
        table.Store(deeperKey, 30, 0, 5, TranspositionBound.Exact);

        await Assert.That(table.TryGet(firstKey, out _)).IsFalse();
        await Assert.That(table.TryGet(secondKey, out _)).IsTrue();
        await Assert.That(table.TryGet(deeperKey, out var deeperEntry)).IsTrue();
        await Assert.That(deeperEntry.Depth).IsEqualTo(5);
    }

    [Test]
    public async Task Store_WhenEntryIsFromPreviousGeneration_ReplacesStaleEntry()
    {
        var table = new TranspositionTable();
        table.Resize(1);
        var staleKey = 1UL;
        var currentKey = staleKey + OneMegabyteBucketCount;
        var replacementKey = staleKey + (2UL * OneMegabyteBucketCount);

        table.Store(staleKey, 10, 0, 8, TranspositionBound.Exact);
        table.NewSearch();
        table.Store(currentKey, 20, 0, 2, TranspositionBound.Exact);
        table.Store(replacementKey, 30, 0, 1, TranspositionBound.Exact);

        await Assert.That(table.TryGet(staleKey, out _)).IsFalse();
        await Assert.That(table.TryGet(currentKey, out _)).IsTrue();
        await Assert.That(table.TryGet(replacementKey, out _)).IsTrue();
    }
}
