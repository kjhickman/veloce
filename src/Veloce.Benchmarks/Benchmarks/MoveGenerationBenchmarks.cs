using BenchmarkDotNet.Attributes;
using Veloce.Core.Models;
using Veloce.Movement;
using Veloce.State;

namespace Veloce.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[BenchmarkCategory("MoveGeneration")]
public class MoveGenerationBenchmarks
{
    private Position _startingPosition = null!;
    private Position _midGamePosition = null!;
    private Position _endGamePosition = null!;

    [IterationSetup]
    public void IterationSetup()
    {
        _startingPosition = new Position();
        _midGamePosition = new Position("r2q1rk1/3n1pb1/pp1p1np1/3Pp2p/2P5/1N2BP2/PP1QB1PP/R4RK1 w - - 1 15");
        _endGamePosition = new Position("1nr5/8/1p4p1/3P1pkp/1PP1p3/7P/4B1P1/R6K w - - 1 39");
    }

    [Benchmark]
    public int GenerateMoves_StartingPosition()
    {
        Span<Move> movesBuffer = stackalloc Move[218];
        return MoveGeneration.GenerateLegalMoves(_startingPosition, movesBuffer);
    }

    [Benchmark]
    public int GenerateMoves_MidGamePosition()
    {
        Span<Move> movesBuffer = stackalloc Move[218];
        return MoveGeneration.GenerateLegalMoves(_midGamePosition, movesBuffer);
    }

    [Benchmark]
    public int GenerateMoves_EndGamePosition()
    {
        Span<Move> movesBuffer = stackalloc Move[218];
        return MoveGeneration.GenerateLegalMoves(_endGamePosition, movesBuffer);
    }
}
