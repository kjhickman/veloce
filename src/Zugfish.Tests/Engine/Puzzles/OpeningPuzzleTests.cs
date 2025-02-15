using Zugfish.Engine;

namespace Zugfish.Tests.Engine.Puzzles;

public class OpeningPuzzleTests
{
    private readonly Search _search = new();

    // https://lichess.org/training/Km0QH
    [Fact]
    public void Puzzle1()
    {
        var position = new Position("r1b2rk1/ppp1qpp1/2p1Pn1p/2b5/2Q5/2N2N1P/PPP2PP1/R1B1R1K1 w - - 1 14");
        var bestMove = _search.FindBestMove(position, 4);
        Assert.NotNull(bestMove);
        Assert.Equal("e6f7", Helpers.UciFromMove(bestMove.Value));

        position.MakeMove(bestMove.Value);
        position.MakeMove("e7f7");

        bestMove = _search.FindBestMove(position, 4);
        Assert.NotNull(bestMove);
        Assert.Equal("c4c5", Helpers.UciFromMove(bestMove.Value));
    }
}
