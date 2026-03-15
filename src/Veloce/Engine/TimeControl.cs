using System.Runtime.InteropServices;

namespace Veloce.Engine;

[StructLayout(LayoutKind.Auto)]
public readonly struct TimeControl(int timeLeft, int increment = -1, int movesToGo = -1)
{
    public int TimeLeft { get; } = timeLeft;
    public int Increment { get; } = increment;
    public int MovesToGo { get; } = movesToGo;

    public static TimeControl Infinite => new(-1);
}
