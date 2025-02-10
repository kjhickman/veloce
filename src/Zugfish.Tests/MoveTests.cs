using Zugfish.Engine;

namespace Zugfish.Tests;

public class MoveTests
{
    [Fact]
    public void GetMoveType_GivenCastlingMove_ReturnsCastling()
    {
        var move = new Move(4, 6, MoveType.Castling);

        var result = move.GetMoveType();

        Assert.Equal(MoveType.Castling, result);
    }
}
