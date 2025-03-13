using Veloce;
using Veloce.Uci.Lib.Extensions;

namespace Veloce.Tests.Engine.Puzzles;

public class CheckmatePuzzleTests
{
    private readonly Search _search = new();
    private readonly MoveExecutor _executor = new();

    // https://lichess.org/training/pipKp
    [Fact]
    public void Puzzle1()
    {
        var position = new Position("2rq3r/3kbpp1/3p3p/4p1P1/p1Q4P/P1n1BN2/2P2P2/2KR3R w - - 2 22");
        var bestMove = _search.FindBestMove(position, 4).BestMove;
        Assert.NotNull(bestMove);
        Assert.Equal("f3e5", Helpers.UciFromMove(bestMove.Value));

        _executor.MakeMove(position, bestMove.Value);
        _executor.MakeMove(position, "d7e8");

        bestMove = _search.FindBestMove(position, 4).BestMove;
        Assert.NotNull(bestMove);
        Assert.Equal("c4f7", Helpers.UciFromMove(bestMove.Value));
    }
}
