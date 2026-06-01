using ChessLite.Movement;
using ChessLite.Primitives;
using Veloce.Uci;

namespace Veloce.UnitTests;

public class UciMoveFormatterTests
{
    [Test]
    public async Task Format_KnightPromotion_UsesUciKnightCharacter()
    {
        var move = Move.CreatePromotion(Square.a7, Square.a8, PieceType.WhitePawn, PromotedPieceType.Knight);

        await Assert.That(UciMoveFormatter.Format(move)).IsEqualTo("a7a8n");
    }
}
