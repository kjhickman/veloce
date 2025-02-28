using Shouldly;
using Zugfish.Engine;
using Zugfish.Engine.Models;

namespace Zugfish.Tests.Engine.MoveGeneratorTests;

public class PerftProblemTests
{
    [Fact]
    public void Problem1()
    {
        var position = new Position("8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1");
        position.MakeMove("b4a4");
        position.MakeMove("c7c5");

        // Act
        Span<Move> moves = stackalloc Move[218];
        var moveCount = MoveGeneration.GenerateLegalMoves(position, moves);

        // Assert
        moveCount.ShouldBe(15);
    }
}
