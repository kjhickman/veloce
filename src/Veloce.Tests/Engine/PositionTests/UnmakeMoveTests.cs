﻿using Veloce.Uci.Lib.Extensions;

namespace Veloce.Tests.Engine.PositionTests;

public class UnmakeMoveTests
{
    /// <summary>
    /// Helper that asserts two boards have identical piece placements,
    /// castling rights, and en passant target.
    /// </summary>
    private static void AssertPositionsEqual(Position expected, Position actual)
    {
        Assert.Equal(expected.WhitePawns.Value, actual.WhitePawns.Value);
        Assert.Equal(expected.WhiteKnights.Value, actual.WhiteKnights.Value);
        Assert.Equal(expected.WhiteBishops.Value, actual.WhiteBishops.Value);
        Assert.Equal(expected.WhiteRooks.Value, actual.WhiteRooks.Value);
        Assert.Equal(expected.WhiteQueens.Value, actual.WhiteQueens.Value);
        Assert.Equal(expected.WhiteKing.Value, actual.WhiteKing.Value);

        Assert.Equal(expected.BlackPawns.Value, actual.BlackPawns.Value);
        Assert.Equal(expected.BlackKnights.Value, actual.BlackKnights.Value);
        Assert.Equal(expected.BlackBishops.Value, actual.BlackBishops.Value);
        Assert.Equal(expected.BlackRooks.Value, actual.BlackRooks.Value);
        Assert.Equal(expected.BlackQueens.Value, actual.BlackQueens.Value);
        Assert.Equal(expected.BlackKing.Value, actual.BlackKing.Value);

        Assert.Equal(expected.CastlingRights, actual.CastlingRights);
        Assert.Equal(expected.EnPassantTarget, actual.EnPassantTarget);
    }

    [Fact]
    public void UnmakeMove_KingsideCastlingWhite_RestoresOriginalPosition()
    {
        // Set up a position with only the white king on e1 and a white rook on h1.
        const string fen = "3k4/8/8/8/8/8/8/4K2R w K - 0 1";
        var position = new Position(fen);
        var expected = new Position(fen);
        var executor = new MoveExecutor();

        // White castling kingside (e1→g1)
        executor.MakeMove(position, "e1g1");
        executor.UndoMove(position);

        // After unmaking, the position should match the starting state.
        AssertPositionsEqual(expected, position);
    }

    [Fact]
    public void UnmakeMove_QueensideCastlingWhite_RestoresOriginalPosition()
    {
        // Set up a position with the white king on e1 and a white rook on a1.
        const string fen = "3k4/8/8/8/8/8/8/R3K3 w Q - 0 1";
        var position = new Position(fen);
        var expected = new Position(fen);
        var executor = new MoveExecutor();

        // White queenside castling (e1→c1)
        executor.MakeMove(position, "e1c1");
        executor.UndoMove(position);

        AssertPositionsEqual(expected, position);
    }

    [Fact]
    public void UnmakeMove_KingsideCastlingBlack_RestoresOriginalPosition()
    {
        // Set up a position with the black king on e8 and a black rook on h8.
        const string fen = "4k2r/8/8/8/8/8/8/4K3 b k - 0 1";
        var position = new Position(fen);
        var expected = new Position(fen);
        var executor = new MoveExecutor();

        // Black kingside castling: e8 → g8.
        executor.MakeMove(position, "e8g8");
        executor.UndoMove(position);

        AssertPositionsEqual(expected, position);
    }

    [Fact]
    public void UnmakeMove_QueensideCastlingBlack_RestoresOriginalPosition()
    {
        // Set up a position with the black king on e8 and a black rook on a8.
        const string fen = "r3k3/8/8/8/8/8/8/4K3 w q - 0 1";
        var position = new Position(fen);
        var expected = new Position(fen);
        var executor = new MoveExecutor();

        // Black queenside castling: e8 → c8.
        executor.MakeMove(position, "e8c8");
        executor.UndoMove(position);

        AssertPositionsEqual(expected, position);
    }

    [Fact]
    public void UnmakeMove_DoublePawnPushWhite_RestoresOriginalPosition()
    {
        // Using the standard starting position:
        // A double pawn push from e2 to e4 should set en passant to e3.
        var position = new Position();
        var expected = new Position();
        var executor = new MoveExecutor();

        executor.MakeMove(position, "e2e4");
        executor.UndoMove(position);

        AssertPositionsEqual(expected, position);
    }

    [Fact]
    public void UnmakeMove_DoublePawnPushBlack_RestoresOriginalPosition()
    {
        // In the standard starting position, white moves first.
        // After a dummy white move, a black pawn from e7 moves to e5,
        // which should set the en passant target to e6.
        var position = new Position();
        var executor = new MoveExecutor();
        executor.MakeMove(position, "a2a3"); // White non-interfering move.

        // Capture the state after white's move as the expected state for black's move.
        var expected = new Position();
        var executor2 = new MoveExecutor();

        executor2.MakeMove(expected, "a2a3");

        executor.MakeMove(position, "e7e5");
        executor.UndoMove(position);

        AssertPositionsEqual(expected, position);
    }

    [Theory]
    [InlineData("q")]
    [InlineData("r")]
    [InlineData("b")]
    [InlineData("n")]
    public void UnmakeMove_PromotionWhite_RestoresOriginalPosition(string promo)
    {
        // Create a position with a white pawn on g7 (ready to promote) and a black king.
        const string fen = "3k4/6P1/8/8/8/8/8/3K4 w - - 0 1";
        var position = new Position(fen);
        var expected = new Position(fen);
        var executor = new MoveExecutor();

        var move = $"g7g8{promo}";
        executor.MakeMove(position, move);
        executor.UndoMove(position);

        AssertPositionsEqual(expected, position);
    }

    [Theory]
    [InlineData("q")]
    [InlineData("r")]
    [InlineData("b")]
    [InlineData("n")]
    public void UnmakeMove_PromotionBlack_RestoresOriginalPosition(string promo)
    {
        // Create a position with a black pawn on a2 (ready to promote) and a white king.
        const string fen = "3k4/8/8/8/8/8/p7/3K4 b - - 0 1";
        var position = new Position(fen);
        var expected = new Position(fen);
        var executor = new MoveExecutor();

        var move = $"a2a1{promo}";
        executor.MakeMove(position, move);
        executor.UndoMove(position);

        AssertPositionsEqual(expected, position);
    }

    [Fact]
    public void UnmakeMove_EnPassantWhite_RestoresOriginalPosition()
    {
        // Set up a position where a white pawn on d5 can capture en passant a black pawn on e5.
        const string fen = "4k3/8/8/3Pp3/8/8/8/4K3 w - e6 0 1";
        var position = new Position(fen);
        var expected = new Position(fen);
        var executor = new MoveExecutor();

        executor.MakeMove(position, "d5e6");
        executor.UndoMove(position);

        AssertPositionsEqual(expected, position);
    }

    [Fact]
    public void UnmakeMove_EnPassantBlack_RestoresOriginalPosition()
    {
        // Set up a position where a black pawn on d4 can capture en passant a white pawn on e4.
        const string fen = "8/8/8/8/3pP3/8/8/8 b - e3 0 1";
        var position = new Position(fen);
        var expected = new Position(fen);
        var executor = new MoveExecutor();

        executor.MakeMove(position, "d4e3");
        executor.UndoMove(position);

        AssertPositionsEqual(expected, position);
    }

    [Fact]
    public void UnmakeMove_WhenKnightCaptures_RestoresOriginalPosition()
    {
        // Set up a position where a white knight on f3 captures a black pawn on e5.
        const string fen = "rnbqkbnr/ppp2ppp/8/3pp3/4P3/5N2/PPPP1PPP/RNBQKB1R w KQkq - 0 3";
        var position = new Position(fen);
        var expected = new Position(fen);
        var executor = new MoveExecutor();

        executor.MakeMove(position, "f3e5");
        var foo = new Position("rnbqkbnr/ppp2ppp/8/3pN3/4P3/8/PPPP1PPP/RNBQKB1R b KQkq - 0 3");
        AssertPositionsEqual(foo, position);

        executor.UndoMove(position);

        AssertPositionsEqual(expected, position);
    }
}
