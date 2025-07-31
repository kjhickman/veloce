﻿using Veloce.Engine;
using Veloce.State;
using Veloce.Uci.Lib.Extensions;

namespace Veloce.UnitTests.Engine.Puzzles;

public class OpeningPuzzleTests
{
    private readonly Search.Search _search = new(settings: new EngineSettings { MaxDepth = 4 });

    // https://lichess.org/training/Km0QH
    [Test]
    public async Task Puzzle1()
    {
        var game = new Game("r1b2rk1/ppp1qpp1/2p1Pn1p/2b5/2Q5/2N2N1P/PPP2PP1/R1B1R1K1 w - - 1 14");
        var bestMove = _search.FindBestMove(game).BestMove;
        await Assert.That(bestMove.ToString()).IsEqualTo("e6f7");

        game.MakeMove(bestMove!.Value);
        game.MakeMove("e7f7");

        bestMove = _search.FindBestMove(game).BestMove;
        await Assert.That(bestMove.ToString()).IsEqualTo("c4c5");
    }
}
