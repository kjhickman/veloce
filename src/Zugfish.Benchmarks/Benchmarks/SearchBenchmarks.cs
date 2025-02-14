using BenchmarkDotNet.Attributes;
using Zugfish.Engine;
using Zugfish.Engine.Models;

namespace Zugfish.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[BenchmarkCategory("Search")]
public class SearchBenchmarks
{
    private Position _startingPosition = null!;
    private Position _midGamePosition = null!;
    private Position _endGamePosition = null!;
    private Position _puzzlePosition1 = null!;
    private Position _puzzlePosition2 = null!;
    private Position _puzzlePosition3 = null!;
    private Position _puzzlePosition4 = null!;
    private readonly MoveGenerator _moveGenerator = new();

    // [Params(3, 4)]
    // public int Depth { get; set; }

    [IterationSetup]
    public void IterationSetup()
    {
        _startingPosition = new();
        _midGamePosition = new("r2q1rk1/3n1pb1/pp1p1np1/3Pp2p/2P5/1N2BP2/PP1QB1PP/R4RK1 w - - 1 15");
        _endGamePosition = new("1nr5/8/1p4p1/3P1pkp/1PP1p3/7P/4B1P1/R6K w - - 1 39");
        _puzzlePosition1 = new("2rq3r/3kbpp1/3p3p/4p1P1/p1Q4P/P1n1BN2/2P2P2/2KR3R w - - 2 22");
        _puzzlePosition2 = new("4b3/5pp1/p2k3p/1ppB4/4K3/2P3P1/PP3PP1/8 b - - 7 31");
        _puzzlePosition3 = new("5n1k/pb5r/1p1P2p1/8/2B2b1q/1N2R2P/PP2Q1P1/4R1K1 b - - 0 1");
        _puzzlePosition4 = new("r1b2rk1/ppp1qpp1/2p1Pn1p/2b5/2Q5/2N2N1P/PPP2PP1/R1B1R1K1 w - - 1 14");
    }

    [Benchmark]
    public Move FindBestMove_StartingPosition()
    {
        var bestMove = Search.FindBestMove(_moveGenerator, _startingPosition, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move FindBestMove_MidGame()
    {
        var bestMove = Search.FindBestMove(_moveGenerator, _midGamePosition, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move FindBestMove_EndGame()
    {
        var bestMove = Search.FindBestMove(_moveGenerator, _endGamePosition, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move FindBestMove_Puzzle1()
    {
        var bestMove = Search.FindBestMove(_moveGenerator, _puzzlePosition1, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move FindBestMove_Puzzle2()
    {
        var bestMove = Search.FindBestMove(_moveGenerator, _puzzlePosition2, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move FindBestMove_Puzzle3()
    {
        var bestMove = Search.FindBestMove(_moveGenerator, _puzzlePosition3, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move FindBestMove_Puzzle4()
    {
        var bestMove = Search.FindBestMove(_moveGenerator, _puzzlePosition4, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }
}
