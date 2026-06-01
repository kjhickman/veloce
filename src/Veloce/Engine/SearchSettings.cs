namespace Veloce.Engine;

public sealed record SearchSettings(int Depth, TimeSpan? MoveTime = null, bool Infinite = false)
{
    public static SearchSettings Default { get; } = new(3);

    public static SearchSettings Timed(TimeSpan moveTime)
    {
        return new SearchSettings(int.MaxValue, moveTime);
    }
}
