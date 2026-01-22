using BenchmarkDotNet.Attributes;
using ChessLite;
using PerftClass = Veloce.Perft.Perft; // todo: rename

namespace Veloce.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[BenchmarkCategory("Perft")]
public class PerftBenchmarks
{
    private Game _startingGame = null!;

    [IterationSetup]
    public void IterationSetup()
    {
        _startingGame = new Game();
    }

    [Benchmark(Description = "Perft(6) - Starting Position")]
    public long Perft_Depth6_StartingPosition()
    {
        return PerftClass.CountNodes(_startingGame, 6);
    }
}
