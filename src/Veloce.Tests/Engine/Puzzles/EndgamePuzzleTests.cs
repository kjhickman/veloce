using Shouldly;
using Veloce.Models;
using Veloce.Uci.Lib.Extensions;

namespace Veloce.Tests.Engine.Puzzles;

public class EndgamePuzzleTests
{
    private readonly Search _search = new();

    // https://lichess.org/training/CNQTv
    [Fact]
    public void DiscoveredAttack()
    {
        var game = new Game("4b3/5pp1/p2k3p/1ppB4/4K3/2P3P1/PP3PP1/8 b - - 7 31");
        var bestMove = _search.FindBestMove(game, 4).BestMove;
        Assert.NotNull(bestMove);
        Assert.Equal("f7f5", Helpers.UciFromMove(bestMove.Value));

        game.MakeMove(bestMove.Value);
        game.MakeMove("e4f5");

        bestMove = _search.FindBestMove(game, 4).BestMove;
        Assert.NotNull(bestMove);
        Assert.Equal("d6d5", Helpers.UciFromMove(bestMove.Value));
    }

    // https://lichess.org/training/L52ND
    [Fact]
    public void UnderPromotion()
    {
        var game = new Game("4rr2/1pkP4/p3pQ2/2P5/3R4/P7/5KP1/1q6 w - - 1 35");
        var bestMove = _search.FindBestMove(game, 4).BestMove;
        bestMove.ShouldNotBeNull();
        bestMove.Value.PromotedPieceType.ShouldBe(PromotedPieceType.Knight);
        bestMove.Value.To.ShouldBe(Square.e8);
    }
}
