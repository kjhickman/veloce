using BenchmarkDotNet.Attributes;
using ChessLite;
using ChessLite.Parsing;

namespace Veloce.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[BenchmarkCategory("UndoMove")]
public class UndoMoveBenchmarks
{
    private Game _quietMoveGame = null!;
    private Game _doublePawnPushMoveGame = null!;
    private Game _captureMoveGame = null!;
    private Game _castlingMoveGame = null!;
    private Game _enPassantMoveGame = null!;
    private Game _promotionMoveGame = null!;

    [IterationSetup]
    public void IterationSetup()
    {
        _quietMoveGame = new();
        _doublePawnPushMoveGame = new();
        _captureMoveGame = new(Fen.Parse("rnbqkbnr/ppp1pppp/8/3p4/2PP4/8/PP2PPPP/RNBQKBNR b KQkq - 0 2"));
        _castlingMoveGame = new(Fen.Parse("rnbqk2r/pp3ppp/4p3/8/3PP3/5N2/P2Q1PPP/R3KB1R b KQkq - 0 10"));
        _enPassantMoveGame = new(Fen.Parse("rnbqkbnr/ppp2ppp/4p3/8/1PpP4/2N5/P3PPPP/R1BQKBNR b KQkq b3 0 4"));
        _promotionMoveGame = new(Fen.Parse("8/4P1kp/p4p2/3p4/P1p5/2r4P/5PP1/2n3K1 w - - 1 35"));
        _quietMoveGame.MakeUciMove("e2e3");
        _doublePawnPushMoveGame.MakeUciMove("e2e4");
        _captureMoveGame.MakeUciMove("d5c4");
        _castlingMoveGame.MakeUciMove("e8g8");
        _enPassantMoveGame.MakeUciMove("c4b3");
        _promotionMoveGame.MakeUciMove("e7e8q");
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
    }


    [Benchmark]
    public void UndoQuietMove()
    {
        _quietMoveGame.UndoMove();
    }

    [Benchmark]
    public void UndoDoublePawnPushMove()
    {
        _doublePawnPushMoveGame.UndoMove();
    }

    [Benchmark]
    public void UndoCaptureMove()
    {
        _captureMoveGame.UndoMove();
    }

    [Benchmark]
    public void UndoCastlingMove()
    {
        _castlingMoveGame.UndoMove();
    }

    [Benchmark]
    public void UndoEnPassantMove()
    {
        _enPassantMoveGame.UndoMove();
    }

    [Benchmark]
    public void UndoPromotionMove()
    {
        _promotionMoveGame.UndoMove();
    }
}
