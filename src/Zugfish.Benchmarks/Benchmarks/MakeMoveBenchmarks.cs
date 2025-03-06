using BenchmarkDotNet.Attributes;
using Zugfish.Engine;

namespace Zugfish.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[BenchmarkCategory("MakeMove")]
public class MakeMoveBenchmarks
{
    private Position _quietMovePosition = null!;
    private Position _doublePawnPushMovePosition = null!;
    private Position _captureMovePosition = null!;
    private Position _castlingMovePosition = null!;
    private Position _enPassantMovePosition = null!;
    private Position _promotionMovePosition = null!;
    private MoveExecutor _executor = null!;

    [IterationSetup]
    public void IterationSetup()
    {
        _quietMovePosition = new();
        _doublePawnPushMovePosition = new();
        _captureMovePosition = new("rnbqkbnr/ppp1pppp/8/3p4/2PP4/8/PP2PPPP/RNBQKBNR b KQkq - 0 2");
        _castlingMovePosition = new("rnbqk2r/pp3ppp/4p3/8/3PP3/5N2/P2Q1PPP/R3KB1R b KQkq - 0 10");
        _enPassantMovePosition = new("rnbqkbnr/ppp2ppp/4p3/8/1PpP4/2N5/P3PPPP/R1BQKBNR b KQkq b3 0 4");
        _promotionMovePosition = new("8/4P1kp/p4p2/3p4/P1p5/2r4P/5PP1/2n3K1 w - - 1 35");
        _executor = new();
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        _executor.ClearMoveHistory();
    }

    [Benchmark]
    public void MakeQuietMove()
    {
        _executor.MakeMove(_quietMovePosition, "e2e3");
    }

    [Benchmark]
    public void MakeDoublePawnPushMove()
    {
        _executor.MakeMove(_doublePawnPushMovePosition, "e2e4");
    }

    [Benchmark]
    public void MakeCaptureMove()
    {
        _executor.MakeMove(_captureMovePosition, "d5c4");
    }

    [Benchmark]
    public void MakeCastlingMove()
    {
        _executor.MakeMove(_castlingMovePosition, "e8g8");
    }

    [Benchmark]
    public void MakeEnPassantMove()
    {
        _executor.MakeMove(_enPassantMovePosition, "c4b3");
    }

    [Benchmark]
    public void MakePromotionMove()
    {
        _executor.MakeMove(_promotionMovePosition, "e7e8q");
    }
}
