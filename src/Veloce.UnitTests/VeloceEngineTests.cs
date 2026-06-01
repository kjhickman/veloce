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

        var result = engine.FindBestMove(new SearchSettings(1));

        await Assert.That(result.BestMove?.ToString()).IsEqualTo("a1a8");
        await Assert.That(result.Score).IsGreaterThan(0);
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
}
