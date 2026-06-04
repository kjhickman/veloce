namespace Veloce.Engine;

public sealed class SearchSettings
{
    public const int UnlimitedDepth = int.MaxValue;

    public static SearchSettings Default { get; } = DepthLimited(8);

    private SearchSettings(int depth, TimeSpan? moveTime, bool infinite, long? nodeLimit, bool ponder)
    {
        Depth = Math.Max(1, depth);
        MoveTime = moveTime;
        Infinite = infinite;
        NodeLimit = nodeLimit;
        Ponder = ponder;
    }

    public int Depth { get; }

    public TimeSpan? MoveTime { get; }

    public bool Infinite { get; }

    public long? NodeLimit { get; }

    public bool Ponder { get; }

    public static SearchSettings DepthLimited(int depth, long? nodeLimit = null, bool ponder = false)
    {
        return new SearchSettings(depth, null, false, nodeLimit, ponder);
    }

    public static SearchSettings Timed(TimeSpan moveTime, int? depth = null, long? nodeLimit = null, bool ponder = false)
    {
        return new SearchSettings(depth ?? UnlimitedDepth, moveTime, false, nodeLimit, ponder);
    }

    public static SearchSettings InfiniteSearch(long? nodeLimit = null, bool ponder = false)
    {
        return new SearchSettings(UnlimitedDepth, null, true, nodeLimit, ponder);
    }
}
