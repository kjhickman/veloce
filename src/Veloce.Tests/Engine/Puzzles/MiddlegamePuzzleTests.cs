using Veloce.Uci.Lib.Extensions;

namespace Veloce.Tests.Engine.Puzzles;

public class MiddlegamePuzzleTests
{
    private readonly Search _search = new();

    // https://lichess.org/training/CNQTv
    [Fact]
    public void Puzzle1()
    {
        var game = new Game("5n1k/pb5r/1p1P2p1/8/2B2b1q/1N2R2P/PP2Q1P1/4R1K1 b - - 0 1");
        var bestMove = _search.FindBestMove(game, 4).BestMove;
        Assert.NotNull(bestMove);
        Assert.Equal("f4e3", Helpers.UciFromMove(bestMove.Value));

        game.MakeMove(bestMove.Value);
        game.MakeMove("e2e3");

        bestMove = _search.FindBestMove(game, 4).BestMove;
        Assert.NotNull(bestMove);
        Assert.Equal("h4c4", Helpers.UciFromMove(bestMove.Value));
    }
}
