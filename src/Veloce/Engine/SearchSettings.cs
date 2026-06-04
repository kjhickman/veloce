namespace Veloce.Engine;

public sealed record SearchSettings(int Depth, TimeSpan? MoveTime = null, bool Infinite = false, long? NodeLimit = null, bool Ponder = false)
{
    public static SearchSettings Default { get; } = new(3);

    public static SearchSettings Timed(TimeSpan moveTime)
    {
        return new SearchSettings(int.MaxValue, moveTime);
    }
}
