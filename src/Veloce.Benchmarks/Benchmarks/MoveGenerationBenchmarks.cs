using BenchmarkDotNet.Attributes;
using ChessLite;
using ChessLite.Movement;
using ChessLite.Parsing;

namespace Veloce.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[BenchmarkCategory("MoveGeneration")]
public class MoveGenerationBenchmarks
{
    private Game _startingGame = null!;
    private Game _midGameGame = null!;
    private Game _endGameGame = null!;

    [IterationSetup]
    public void IterationSetup()
    {
        _startingGame = new Game();
        _midGameGame = new Game(Fen.Parse("r2q1rk1/3n1pb1/pp1p1np1/3Pp2p/2P5/1N2BP2/PP1QB1PP/R4RK1 w - - 1 15"));
        _endGameGame = new Game(Fen.Parse("1nr5/8/1p4p1/3P1pkp/1PP1p3/7P/4B1P1/R6K w - - 1 39"));
    }

    [Benchmark]
    public int GenerateMoves_StartingPosition()
    {
        Span<Move> movesBuffer = stackalloc Move[218];
        return _startingGame.WriteLegalMoves(movesBuffer);
    }

    [Benchmark]
    public int GenerateMoves_MidGamePosition()
    {
        Span<Move> movesBuffer = stackalloc Move[218];
        return _midGameGame.WriteLegalMoves(movesBuffer);
    }

    [Benchmark]
    public int GenerateMoves_EndGamePosition()
    {
        Span<Move> movesBuffer = stackalloc Move[218];
        return _endGameGame.WriteLegalMoves(movesBuffer);
    }
}
