using Veloce.Uci.Lib.Extensions;

namespace Veloce.Tests.Engine.Puzzles;

public class CheckmatePuzzleTests
{
    private readonly Search _search = new();

    // https://lichess.org/training/pipKp
    [Fact]
    public void Puzzle1()
    {
        var game = new Game("2rq3r/3kbpp1/3p3p/4p1P1/p1Q4P/P1n1BN2/2P2P2/2KR3R w - - 2 22");
        var bestMove = _search.FindBestMove(game, 4).BestMove;
        Assert.NotNull(bestMove);
        Assert.Equal("f3e5", Helpers.UciFromMove(bestMove.Value));

        game.MakeMove(bestMove.Value);
        game.MakeMove("d7e8");

        bestMove = _search.FindBestMove(game, 4).BestMove;
        Assert.NotNull(bestMove);
        Assert.Equal("c4f7", Helpers.UciFromMove(bestMove.Value));
    }
}
