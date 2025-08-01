using Veloce.Engine;
using Veloce.State;
using Veloce.Uci.Lib.Extensions;

namespace Veloce.UnitTests.Engine.Puzzles;

public class CheckmatePuzzleTests
{
    private readonly Search.MoveFinder _moveFinder = new(settings: new EngineSettings { MaxDepth = 4 });

    // https://lichess.org/training/pipKp
    [Test]
    public async Task Puzzle1()
    {
        var game = new Game("2rq3r/3kbpp1/3p3p/4p1P1/p1Q4P/P1n1BN2/2P2P2/2KR3R w - - 2 22");
        var bestMove = _moveFinder.FindBestMove(game).BestMove;
        await Assert.That(bestMove).IsNotNull();
        await Assert.That(bestMove.ToString()).IsEqualTo("f3e5");

        game.MakeMove(bestMove!.Value);
        game.MakeMove("d7e8");

        bestMove = _moveFinder.FindBestMove(game).BestMove;
        await Assert.That(bestMove).IsNotNull();
        await Assert.That(bestMove.ToString()).IsEqualTo("c4f7");
    }
}
