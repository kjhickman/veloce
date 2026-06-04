using ChessLite.Movement;
using ChessLite.Primitives;
using Veloce.Engine;
using Veloce.Uci;

namespace Veloce.UnitTests;

public class UciFormattingTests
{
    [Test]
    public async Task FormatMove_KnightPromotion_UsesUciKnightCharacter()
    {
        var move = Move.CreatePromotion(Square.a7, Square.a8, PieceType.WhitePawn, PromotedPieceType.Knight);

        await Assert.That(UciFormatting.FormatMove(move)).IsEqualTo("a7a8n");
    }

    [Test]
    public async Task FormatMove_NullMove_ReturnsUciNullMove()
    {
        await Assert.That(UciFormatting.FormatMove(Move.NullMove)).IsEqualTo("0000");
    }

    [Test]
    public async Task FormatSearchResult_WhenBestMoveIsMissing_OmitsPv()
    {
        var result = new SearchResult(null, 0, 0, 0, TimeSpan.Zero);

        await Assert.That(UciFormatting.FormatSearchResult(result)).IsEqualTo("info depth 0 multipv 1 score cp 0 nodes 0 nps 0 time 0 hashfull 0");
    }
}
