using ChessLite;
using ChessLite.Primitives;
using Shouldly;
using Veloce.Engine;

namespace Veloce.UnitTests;

public class EndgamePuzzleTests
{
    private readonly Search.MoveFinder _moveFinder = new(settings: new EngineSettings { MaxDepth = 4 });

    // https://lichess.org/training/CNQTv
    [Test]
    public async Task DiscoveredAttack()
    {
        var game = Game.FromFen("4b3/5pp1/p2k3p/1ppB4/4K3/2P3P1/PP3PP1/8 b - - 7 31");
        var bestMove = _moveFinder.FindBestMove(game).BestMove;
        await Assert.That(bestMove.ToString()).IsEqualTo("f7f5");

        game.MakeMove(bestMove!.Value);
        game.MakeUciMove("e4f5");

        bestMove = _moveFinder.FindBestMove(game).BestMove;
        await Assert.That(bestMove.ToString()).IsEqualTo("d6d5");
    }

    // https://lichess.org/training/L52ND
    [Test]
    public void UnderPromotion()
    {
        var game = Game.FromFen("4rr2/1pkP4/p3pQ2/2P5/3R4/P7/5KP1/1q6 w - - 1 35");
        var bestMove = _moveFinder.FindBestMove(game).BestMove;
        bestMove.ShouldNotBeNull();
        bestMove.Value.PromotedPieceType.ShouldBe(PromotedPieceType.Knight);
        bestMove.Value.To.ShouldBe(Square.e8);
    }
}
