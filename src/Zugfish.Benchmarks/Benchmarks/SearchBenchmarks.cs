using BenchmarkDotNet.Attributes;
using Zugfish.Engine;
using Zugfish.Engine.Models;

namespace Zugfish.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[BenchmarkCategory("Search")]
public class SearchBenchmarks
{
    private Position _coldStartingPosition = null!;
    private Position _coldMidGamePosition = null!;
    private Position _coldEndGamePosition = null!;
    private Position _coldPuzzlePosition1 = null!;
    private Position _coldPuzzlePosition2 = null!;
    private Position _coldPuzzlePosition3 = null!;
    private Position _coldPuzzlePosition4 = null!;
    private Position _warmStartingPosition = null!;
    private Position _warmMidGamePosition = null!;
    private Position _warmEndGamePosition = null!;
    private Position _warmPuzzlePosition1 = null!;
    private Position _warmPuzzlePosition2 = null!;
    private Position _warmPuzzlePosition3 = null!;
    private Position _warmPuzzlePosition4 = null!;
    private Search _search = null!;

    // [Params(3, 4)]
    // public int Depth { get; set; }

    [IterationSetup]
    public void IterationSetup()
    {
        _search = new Search();

        _coldStartingPosition = new Position();
        _warmStartingPosition = new Position();
        _warmStartingPosition.MakeMove("e2e4");
        _warmStartingPosition.MakeMove("e7e5");

        _coldMidGamePosition = new Position("r2q1rk1/3n1pb1/pp1p1np1/3Pp2p/2P5/1N2BP2/PP1QB1PP/R4RK1 w - - 1 15");
        _warmMidGamePosition = new Position("r2q1rk1/1p1n1pb1/p2p1np1/3Pp2p/8/1NP1BP2/PP1QB1PP/R4RK1 w - - 1 15");
        _search.FindBestMove(_warmMidGamePosition, 3);
        _warmMidGamePosition.MakeMove("c3c4");
        _search.FindBestMove(_warmMidGamePosition, 3);
        _warmMidGamePosition.MakeMove("b7b6");


        _coldEndGamePosition = new Position("1nr5/8/1p4p1/3P1pkp/1PP1p3/7P/4B1P1/R6K w - - 1 39");
        _warmEndGamePosition = new Position("1nr5/8/1p4p1/3P1pkp/1PP1p3/7P/4B1P1/R6K w - - 1 39");
        _warmEndGamePosition.MakeMove("a1a8");
        _warmEndGamePosition.MakeMove("g5f4");

        _coldPuzzlePosition1 = new Position("2rq3r/3kbpp1/3p3p/4p1P1/p1Q4P/P1n1BN2/2P2P2/2KR3R w - - 2 22");
        _warmPuzzlePosition1 = new Position("2rq3r/3kbpp1/3p3p/4p1P1/p1Q4P/P1n1BN2/2P2P2/2KR3R w - - 2 22");
        _warmPuzzlePosition1.MakeMove("f3e5");
        _warmPuzzlePosition1.MakeMove("d7e8");

        _coldPuzzlePosition2 = new Position("4b3/5pp1/p2k3p/1ppB4/4K3/2P3P1/PP3PP1/8 b - - 7 31");
        _warmPuzzlePosition2 = new Position("4b3/5pp1/p2k3p/1ppB4/4K3/2P3P1/PP3PP1/8 b - - 7 31");
        _warmPuzzlePosition2.MakeMove("f7f5");
        _warmPuzzlePosition2.MakeMove("e4f5");

        _coldPuzzlePosition3 = new Position("5n1k/pb5r/1p1P2p1/8/2B2b1q/1N2R2P/PP2Q1P1/4R1K1 b - - 0 1");
        _warmPuzzlePosition3 = new Position("5n1k/pb5r/1p1P2p1/8/2B2b1q/1N2R2P/PP2Q1P1/4R1K1 b - - 0 1");
        _warmPuzzlePosition3.MakeMove("f4e3");
        _warmPuzzlePosition3.MakeMove("e2e3");

        _coldPuzzlePosition4 = new Position("r1b2rk1/ppp1qpp1/2p1Pn1p/2b5/2Q5/2N2N1P/PPP2PP1/R1B1R1K1 w - - 1 14");
        _warmPuzzlePosition4 = new Position("r1b2rk1/ppp1qpp1/2p1Pn1p/2b5/2Q5/2N2N1P/PPP2PP1/R1B1R1K1 w - - 1 14");
        _warmPuzzlePosition4.MakeMove("e6f7");
        _warmPuzzlePosition4.MakeMove("e7f7");
    }

    [Benchmark]
    public Move Cold_FindBestMove_StartingPosition()
    {
        var bestMove = _search.FindBestMove(_coldStartingPosition, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move Cold_FindBestMove_MidGame()
    {
        var bestMove = _search.FindBestMove(_coldMidGamePosition, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move Cold_FindBestMove_EndGame()
    {
        var bestMove = _search.FindBestMove(_coldEndGamePosition, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move Cold_FindBestMove_Puzzle1()
    {
        var bestMove = _search.FindBestMove(_coldPuzzlePosition1, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move Cold_FindBestMove_Puzzle2()
    {
        var bestMove = _search.FindBestMove(_coldPuzzlePosition2, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move Cold_FindBestMove_Puzzle3()
    {
        var bestMove = _search.FindBestMove(_coldPuzzlePosition3, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move Cold_FindBestMove_Puzzle4()
    {
        var bestMove = _search.FindBestMove(_coldPuzzlePosition4, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move Warm_FindBestMove_StartingPosition()
    {
        var bestMove = _search.FindBestMove(_warmStartingPosition, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move Warm_FindBestMove_MidGame()
    {
        var bestMove = _search.FindBestMove(_warmMidGamePosition, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move Warm_FindBestMove_EndGame()
    {
        var bestMove = _search.FindBestMove(_warmEndGamePosition, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move Warm_FindBestMove_Puzzle1()
    {
        var bestMove = _search.FindBestMove(_warmPuzzlePosition1, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move Warm_FindBestMove_Puzzle2()
    {
        var bestMove = _search.FindBestMove(_warmPuzzlePosition2, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move Warm_FindBestMove_Puzzle3()
    {
        var bestMove = _search.FindBestMove(_warmPuzzlePosition3, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }

    [Benchmark]
    public Move Warm_FindBestMove_Puzzle4()
    {
        var bestMove = _search.FindBestMove(_warmPuzzlePosition4, 3);
        return bestMove ?? throw new InvalidOperationException("No move found");
    }
}
