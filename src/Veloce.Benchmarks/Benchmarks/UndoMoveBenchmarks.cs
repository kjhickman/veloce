using BenchmarkDotNet.Attributes;
using Veloce;
using Veloce.Uci.Lib.Extensions;

namespace Veloce.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[BenchmarkCategory("UndoMove")]
public class UndoMoveBenchmarks
{
    private Position _quietMovePosition = null!;
    private Position _doublePawnPushMovePosition = null!;
    private Position _captureMovePosition = null!;
    private Position _castlingMovePosition = null!;
    private Position _enPassantMovePosition = null!;
    private Position _promotionMovePosition = null!;
    private MoveExecutor _executor1 = null!;
    private MoveExecutor _executor2 = null!;
    private MoveExecutor _executor3 = null!;
    private MoveExecutor _executor4 = null!;
    private MoveExecutor _executor5 = null!;
    private MoveExecutor _executor6 = null!;

    [IterationSetup]
    public void IterationSetup()
    {
        _quietMovePosition = new();
        _doublePawnPushMovePosition = new();
        _captureMovePosition = new("rnbqkbnr/ppp1pppp/8/3p4/2PP4/8/PP2PPPP/RNBQKBNR b KQkq - 0 2");
        _castlingMovePosition = new("rnbqk2r/pp3ppp/4p3/8/3PP3/5N2/P2Q1PPP/R3KB1R b KQkq - 0 10");
        _enPassantMovePosition = new("rnbqkbnr/ppp2ppp/4p3/8/1PpP4/2N5/P3PPPP/R1BQKBNR b KQkq b3 0 4");
        _promotionMovePosition = new("8/4P1kp/p4p2/3p4/P1p5/2r4P/5PP1/2n3K1 w - - 1 35");
        _executor1 = new();
        _executor2 = new();
        _executor3 = new();
        _executor4 = new();
        _executor5 = new();
        _executor6 = new();
        _executor1.MakeMove(_quietMovePosition, "e2e3");
        _executor2.MakeMove(_doublePawnPushMovePosition, "e2e4");
        _executor3.MakeMove(_captureMovePosition, "d5c4");
        _executor4.MakeMove(_castlingMovePosition, "e8g8");
        _executor5.MakeMove(_enPassantMovePosition, "c4b3");
        _executor6.MakeMove(_promotionMovePosition, "e7e8q");
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        _executor1.ClearMoveHistory();
        _executor2.ClearMoveHistory();
        _executor3.ClearMoveHistory();
        _executor4.ClearMoveHistory();
        _executor5.ClearMoveHistory();
        _executor6.ClearMoveHistory();
    }


    [Benchmark]
    public void UndoQuietMove()
    {
        _executor1.UndoMove(_quietMovePosition);
    }

    [Benchmark]
    public void UndoDoublePawnPushMove()
    {
        _executor2.UndoMove(_doublePawnPushMovePosition);
    }

    [Benchmark]
    public void UndoCaptureMove()
    {
        _executor3.UndoMove(_captureMovePosition);
    }

    [Benchmark]
    public void UndoCastlingMove()
    {
        _executor4.UndoMove(_castlingMovePosition);
    }

    [Benchmark]
    public void UndoEnPassantMove()
    {
        _executor5.UndoMove(_enPassantMovePosition);
    }

    [Benchmark]
    public void UndoPromotionMove()
    {
        _executor6.UndoMove(_promotionMovePosition);
    }
}
