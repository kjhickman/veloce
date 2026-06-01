namespace Veloce.Engine;

public sealed record SearchSettings(int Depth)
{
    public static SearchSettings Default { get; } = new(3);
}
