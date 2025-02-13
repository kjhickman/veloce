using BenchmarkDotNet.Attributes;
using Zugfish.Engine;

namespace Zugfish.Benchmarks;

[MemoryDiagnoser]
[ShortRunJob]
public class SearchBenchmarks
{
    private readonly Board _puzzleBoard = new("r3kb1r/1b3ppp/p3pn2/1pn1P3/3q4/2NB4/PPP3PP/R1BQ1R1K w kq - 0 1");
    private readonly Board _midGameBoard = new("r1bqk2r/1pp2ppp/2n2n2/p2pN3/QbB5/2N5/PP3PPP/R1B2RK1 w kq - 0 11");
    private readonly MoveGenerator _moveGenerator = new();

    [Params(3, 4, 5)]
    public int Depth { get; set; }

    [Benchmark]
    public Move FindBestMove_Puzzle()
    {
        var bestMove = Search.FindBestMove(_moveGenerator, _puzzleBoard, Depth);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move FindBestMove_MidGame()
    {
        var bestMove = Search.FindBestMove(_moveGenerator, _midGameBoard, Depth);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }
}
