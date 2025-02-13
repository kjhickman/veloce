using BenchmarkDotNet.Attributes;
using Zugfish.Engine;

namespace Zugfish.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class MoveGeneratorBenchmarks
{
    private readonly Board _board = new("r3kb1r/1b3ppp/p3pn2/1pn1P3/3q4/2NB4/PPP3PP/R1BQ1R1K w kq - 0 1");
    private readonly MoveGenerator _moveGenerator = new();

    [Benchmark]
    public Move GenerateMoves()
    {
        Span<Move> movesBuffer = stackalloc Move[218];
        _moveGenerator.GenerateLegalMoves(_board, movesBuffer);
        return movesBuffer[0];
    }
}
