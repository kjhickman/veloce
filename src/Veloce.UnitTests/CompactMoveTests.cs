using ChessLite;
using ChessLite.Movement;
using ChessLite.Parsing;
using ChessLite.Primitives;
using Veloce.Search.Transposition;

namespace Veloce.UnitTests;

public class CompactMoveTests
{
    [Test]
    public async Task FindMatchingMove_WhenNullMove_ReturnsNullMove()
    {
        var compactMove = new CompactMove(Move.NullMove);

        Span<Move> moves = stackalloc Move[218];
        var matchedMove = compactMove.FindMatchingMove(moves, 0);

        await Assert.That(matchedMove).IsEqualTo(Move.NullMove);
    }

    [Test]
    public async Task FindMatchingMove_WhenMoveExists_ReturnsMatchingMove()
    {
        var game = new Game();
        Span<Move> moves = stackalloc Move[218];
        var moveCount = game.WriteLegalMoves(moves);
        var expectedMove = FindMove(moves, moveCount, "e2e4");
        var compactMove = new CompactMove(expectedMove);

        var matchedMove = compactMove.FindMatchingMove(moves, moveCount);

        await Assert.That(matchedMove).IsEqualTo(expectedMove);
    }

    [Test]
    public async Task FindMatchingMove_WhenPromotionExists_MatchesPromotionType()
    {
        var game = new Game(Fen.Parse("4k3/P7/8/8/8/8/8/4K3 w - - 0 1"));
        Span<Move> moves = stackalloc Move[218];
        var moveCount = game.WriteLegalMoves(moves);
        var expectedMove = FindPromotionMove(moves, moveCount, PromotedPieceType.Queen);
        var compactMove = new CompactMove(expectedMove);

        var matchedMove = compactMove.FindMatchingMove(moves, moveCount);

        await Assert.That(matchedMove).IsEqualTo(expectedMove);
    }

    private static Move FindMove(ReadOnlySpan<Move> moves, int moveCount, string uciMove)
    {
        for (var i = 0; i < moveCount; i++)
        {
            if (moves[i].ToString() == uciMove) return moves[i];
        }

        throw new InvalidOperationException($"Move not found: {uciMove}");
    }

    private static Move FindPromotionMove(ReadOnlySpan<Move> moves, int moveCount, PromotedPieceType promotedPieceType)
    {
        for (var i = 0; i < moveCount; i++)
        {
            if (moves[i].PromotedPieceType == promotedPieceType) return moves[i];
        }

        throw new InvalidOperationException($"Promotion move not found: {promotedPieceType}");
    }
}
