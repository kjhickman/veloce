using ChessLite.Movement;
using ChessLite.Parsing;
using Veloce.Engine;

namespace Veloce.UnitTests;

public class VeloceEngineTests
{
    [Test]
    public async Task FindBestMove_FromStartPosition_ReturnsLegalMove()
    {
        var engine = new VeloceEngine();

        var result = engine.FindBestMove();

        await Assert.That(result.BestMove.HasValue).IsTrue();
        await Assert.That(IsLegal(engine, result.BestMove!.Value)).IsTrue();
        await Assert.That(result.Depth).IsEqualTo(SearchSettings.Default.Depth);
        await Assert.That(result.Nodes).IsGreaterThan(0);
    }

    [Test]
    public async Task FindBestMove_FromFen_ReturnsLegalMove()
    {
        var engine = new VeloceEngine();
        engine.SetPosition(Fen.Parse("r2q1rk1/3n1pb1/pp1p1np1/3Pp2p/2P5/1N2BP2/PP1QB1PP/R4RK1 w - - 1 15"));

        var result = engine.FindBestMove();

        await Assert.That(result.BestMove.HasValue).IsTrue();
        await Assert.That(IsLegal(engine, result.BestMove!.Value)).IsTrue();
    }

    [Test]
    public async Task FindBestMove_WhenQueenCanBeCaptured_SelectsCapture()
    {
        var engine = new VeloceEngine();
        engine.SetPosition(Fen.Parse("q3k3/8/8/8/8/8/8/R3K3 w - - 0 1"));

        var result = engine.FindBestMove(SearchSettings.DepthLimited(1));

        await Assert.That(result.BestMove?.ToString()).IsEqualTo("a1a8");
        await Assert.That(result.Score).IsGreaterThan(0);
    }

    [Test]
    public async Task FindBestMove_WhenCaptureLosesQueen_AvoidsCaptureAfterQuiescence()
    {
        var engine = new VeloceEngine();
        engine.SetPosition(Fen.Parse("rk6/8/8/8/8/8/8/Q3K3 w - - 0 1"));

        var result = engine.FindBestMove(SearchSettings.DepthLimited(1));

        await Assert.That(result.BestMove?.ToString()).IsNotEqualTo("a1a8");
    }

    [Test]
    public async Task FindBestMove_WhenCurrentPositionIsDrawnByRepetition_ScoresDraw()
    {
        var engine = new VeloceEngine();
        engine.SetPosition(Fen.Parse("4k1n1/8/8/8/8/8/8/1N2K3 w - - 0 1"));
        engine.MakeUciMove("b1d2");
        engine.MakeUciMove("g8f6");
        engine.MakeUciMove("d2b1");
        engine.MakeUciMove("f6g8");
        engine.MakeUciMove("b1d2");
        engine.MakeUciMove("g8f6");
        engine.MakeUciMove("d2b1");
        engine.MakeUciMove("f6g8");

        var result = engine.FindBestMove(SearchSettings.DepthLimited(1));

        await Assert.That(result.BestMove.HasValue).IsFalse();
        await Assert.That(result.Score).IsEqualTo(0);
    }

    [Test]
    public async Task FindBestMove_WithMoveTime_ReturnsLegalMove()
    {
        var engine = new VeloceEngine();

        var result = engine.FindBestMove(SearchSettings.Timed(TimeSpan.FromMilliseconds(100)));

        await Assert.That(result.BestMove.HasValue).IsTrue();
        await Assert.That(IsLegal(engine, result.BestMove!.Value)).IsTrue();
        await Assert.That(result.Depth).IsGreaterThanOrEqualTo(1);
    }

    [Test]
    public async Task FindBestMove_WhenPositionSearchedAgain_UsesTranspositionTable()
    {
        var engine = new VeloceEngine();
        engine.SetPosition(Fen.Parse("r2q1rk1/3n1pb1/pp1p1np1/3Pp2p/2P5/1N2BP2/PP1QB1PP/R4RK1 w - - 1 15"));

        var first = engine.FindBestMove(SearchSettings.DepthLimited(3));
        var second = engine.FindBestMove(SearchSettings.DepthLimited(3));

        await Assert.That(second.BestMove).IsEqualTo(first.BestMove);
        await Assert.That(second.Nodes).IsLessThan(first.Nodes);
    }

    [Test]
    public async Task FindBestMove_WithMultipleThreads_ReturnsLegalMoveWithoutMutatingPosition()
    {
        var engine = new VeloceEngine();
        engine.SetThreadCount(2);
        var legalMovesBefore = CountLegalMoves(engine);

        var result = engine.FindBestMove(SearchSettings.DepthLimited(2));

        await Assert.That(result.BestMove.HasValue).IsTrue();
        await Assert.That(IsLegal(engine, result.BestMove!.Value)).IsTrue();
        await Assert.That(CountLegalMoves(engine)).IsEqualTo(legalMovesBefore);
    }

    [Arguments(0, 1)]
    [Arguments(1, 20)]
    [Arguments(2, 400)]
    [Arguments(3, 8902)]
    [Test]
    public async Task Perft_FromStartPosition_ReturnsKnownNodeCount(int depth, long expectedNodes)
    {
        var engine = new VeloceEngine();

        var nodes = engine.Perft(depth);

        await Assert.That(nodes).IsEqualTo(expectedNodes);
    }

    [Test]
    public async Task Perft_ReportsRootMoveCounts()
    {
        var engine = new VeloceEngine();
        var rootCounts = new Dictionary<string, long>();

        var nodes = engine.Perft(2, (move, moveNodes) => rootCounts.Add(move.ToString(), moveNodes));

        await Assert.That(nodes).IsEqualTo(400);
        await Assert.That(rootCounts).ContainsKey("e2e4");
        await Assert.That(rootCounts["e2e4"]).IsEqualTo(20);
    }

    private static bool IsLegal(VeloceEngine engine, Move move)
    {
        Span<Move> moves = stackalloc Move[218];
        var count = engine.WriteLegalMoves(moves);

        for (var i = 0; i < count; i++)
        {
            if (moves[i] == move) return true;
        }

        return false;
    }

    private static int CountLegalMoves(VeloceEngine engine)
    {
        Span<Move> moves = stackalloc Move[218];
        return engine.WriteLegalMoves(moves);
    }
}
