using System.Runtime.InteropServices;
using ChessLite;

namespace Veloce.Search;

[StructLayout(LayoutKind.Auto)]
public readonly struct EvaluationResult(int score, GameState state)
{
    public int Score { get; } = score;
    public GameState State { get; init; } = state;
}