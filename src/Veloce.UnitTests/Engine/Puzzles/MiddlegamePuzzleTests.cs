﻿using Veloce.Uci.Lib;
using Veloce.Uci.Lib.Extensions;

namespace Veloce.UnitTests.Engine.Puzzles;

public class MiddlegamePuzzleTests
{
    private readonly Search _search = new(settings: new EngineSettings { MaxDepth = 4 });

    // https://lichess.org/training/CNQTv
    [Test]
    public async Task Puzzle1()
    {
        var game = new Game("5n1k/pb5r/1p1P2p1/8/2B2b1q/1N2R2P/PP2Q1P1/4R1K1 b - - 0 1");
        var bestMove = _search.FindBestMove(game).BestMove;
        await Assert.That(bestMove.ToString()).IsEqualTo("f4e3");

        game.MakeMove(bestMove!.Value);
        game.MakeMove("e2e3");

        bestMove = _search.FindBestMove(game).BestMove;
        await Assert.That(bestMove.ToString()).IsEqualTo("h4c4");
    }
}
