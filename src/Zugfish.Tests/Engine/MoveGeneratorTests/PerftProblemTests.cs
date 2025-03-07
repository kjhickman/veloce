using Shouldly;
using Zugfish.Engine;
using Zugfish.Engine.Models;

namespace Zugfish.Tests.Engine.MoveGeneratorTests;

public class PerftProblemTests
{
    [Fact]
    public void Problem1()
    {
        var position = new Position("8/8/8/1Ppp3r/RK3p1k/8/4P1P1/8 w - c6 0 3");
        // var executor = new MoveExecutor();

        // Act
        Span<Move> moves = stackalloc Move[218];
        var moveCount = MoveGeneration.GenerateLegalMoves(position, moves);

        // Assert
        moveCount.ShouldBe(6);
    }
}
