using Zugfish.Engine;

namespace Zugfish.Tests.Engine.Puzzles;

public class MiddlegamePuzzleTests
{
    private readonly MoveGenerator _moveGenerator = new();

    // https://lichess.org/training/CNQTv
    [Fact]
    public void Puzzle1()
    {
        var position = new Position("5n1k/pb5r/1p1P2p1/8/2B2b1q/1N2R2P/PP2Q1P1/4R1K1 b - - 0 1");
        var bestMove = Search.FindBestMove(_moveGenerator, position, 4);
        Assert.NotNull(bestMove);
        Assert.Equal("f4e3", Helpers.UciFromMove(bestMove.Value));

        position.MakeMove(bestMove.Value);
        position.MakeMove("e2e3");

        bestMove = Search.FindBestMove(_moveGenerator, position, 4);
        Assert.NotNull(bestMove);
        Assert.Equal("h4c4", Helpers.UciFromMove(bestMove.Value));
    }
}
