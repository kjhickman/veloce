using ChessLite.Movement;
using ChessLite.Parsing;
using Veloce.Engine;

namespace Veloce.UnitTests;

public class VeloceEngineTests
{
    [Test]
    public async Task FindBestMove_FromStartPosition_ReturnsLegalMove()
    {
        var engine = new VeloceEngine(new Random(0));

        var move = engine.FindBestMove();

        await Assert.That(move.HasValue).IsTrue();
        await Assert.That(IsLegal(engine, move!.Value)).IsTrue();
    }

    [Test]
    public async Task FindBestMove_FromFen_ReturnsLegalMove()
    {
        var engine = new VeloceEngine(new Random(0));
        engine.SetPosition(Fen.Parse("r2q1rk1/3n1pb1/pp1p1np1/3Pp2p/2P5/1N2BP2/PP1QB1PP/R4RK1 w - - 1 15"));

        var move = engine.FindBestMove();

        await Assert.That(move.HasValue).IsTrue();
        await Assert.That(IsLegal(engine, move!.Value)).IsTrue();
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
