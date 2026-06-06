using ChessLite;
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

        await Assert.That(UciFormatting.FormatSearchResult(result)).IsEqualTo("info depth 0 seldepth 0 multipv 1 score cp 0 nodes 0 nps 0 time 0 hashfull 0");
    }

    [Test]
    public async Task FormatSearchResult_WithPrincipalVariation_FormatsAllMoves()
    {
        var game = new Game();
        var firstMove = FindMove(game, "e2e4");
        game.MakeMove(firstMove);
        var principalVariation = new[] { firstMove, FindMove(game, "e7e5") };
        var result = new SearchResult(principalVariation[0], 34, 2, 100, TimeSpan.FromMilliseconds(10), 0, 3, principalVariation);

        await Assert.That(UciFormatting.FormatSearchResult(result)).IsEqualTo("info depth 2 seldepth 3 multipv 1 score cp 34 nodes 100 nps 10000 time 10 hashfull 0 pv e2e4 e7e5");
    }

    [Test]
    public async Task FormatSearchResult_WithWinningMateScore_UsesMateNotation()
    {
        var result = new SearchResult(FindMove(new Game(), "e2e4"), 99_997, 3, 100, TimeSpan.Zero);

        await Assert.That(UciFormatting.FormatSearchResult(result)).Contains("score mate 2");
    }

    [Test]
    public async Task FormatSearchResult_WithLosingMateScore_UsesMateNotation()
    {
        var result = new SearchResult(FindMove(new Game(), "e2e4"), -99_997, 3, 100, TimeSpan.Zero);

        await Assert.That(UciFormatting.FormatSearchResult(result)).Contains("score mate -2");
    }

    private static Move FindMove(Game game, string uciMove)
    {
        Span<Move> moves = stackalloc Move[218];
        var moveCount = game.WriteLegalMoves(moves);
        for (var i = 0; i < moveCount; i++)
        {
            if (moves[i].ToString() == uciMove) return moves[i];
        }

        throw new InvalidOperationException($"Move not found: {uciMove}");
    }
}
