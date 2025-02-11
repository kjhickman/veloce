using Zugfish.Engine;

namespace Zugfish.Tests;

public class MoveTests
{
    [Fact]
    public void Type_GivenCastlingMove_ReturnsCastling()
    {
        var move = new Move(4, 6, MoveType.Castling);

        var result = move.Type;

        Assert.Equal(MoveType.Castling, result);
    }
}
