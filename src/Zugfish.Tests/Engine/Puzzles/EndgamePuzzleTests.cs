using Zugfish.Engine;

namespace Zugfish.Tests.Engine.Puzzles;

public class EndgamePuzzleTests
{
    private readonly MoveGenerator _moveGenerator = new();

    // https://lichess.org/training/CNQTv
    [Fact]
    public void Puzzle1()
    {
        var position = new Position("4b3/5pp1/p2k3p/1ppB4/4K3/2P3P1/PP3PP1/8 b - - 7 31");
        var bestMove = Search.FindBestMove(_moveGenerator, position, 4);
        Assert.NotNull(bestMove);
        Assert.Equal("f7f5", Helpers.UciFromMove(bestMove.Value));

        position.MakeMove(bestMove.Value);
        position.MakeMove("e4f5");

        bestMove = Search.FindBestMove(_moveGenerator, position, 4);
        Assert.NotNull(bestMove);
        Assert.Equal("d6d5", Helpers.UciFromMove(bestMove.Value));
    }
}
