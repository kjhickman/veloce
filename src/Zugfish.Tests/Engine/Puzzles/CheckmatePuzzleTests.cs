using Zugfish.Engine;

namespace Zugfish.Tests.Engine.Puzzles;

public class CheckmatePuzzleTests
{
    private readonly MoveGenerator _moveGenerator = new();

    // https://lichess.org/training/pipKp
    [Fact]
    public void Puzzle1()
    {
        var position = new Position("2rq3r/3kbpp1/3p3p/4p1P1/p1Q4P/P1n1BN2/2P2P2/2KR3R w - - 2 22");
        var bestMove = Search.FindBestMove(_moveGenerator, position, 4);
        Assert.NotNull(bestMove);
        Assert.Equal("f3e5", Helpers.UciFromMove(bestMove.Value));

        position.MakeMove(bestMove.Value);
        position.MakeMove("d7e8");

        bestMove = Search.FindBestMove(_moveGenerator, position, 4);
        Assert.NotNull(bestMove);
        Assert.Equal("c4f7", Helpers.UciFromMove(bestMove.Value));
    }
}
