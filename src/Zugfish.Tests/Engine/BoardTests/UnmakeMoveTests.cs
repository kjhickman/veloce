using Zugfish.Engine;

namespace Zugfish.Tests.Engine.BoardTests;

public class UnmakeMoveTests
{
    /// <summary>
    /// Helper that asserts two boards have identical piece placements,
    /// castling rights, and en passant target.
    /// </summary>
    private static void AssertBoardsEqual(Board expected, Board actual)
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
        // Set up a board with only the white king on e1 and a white rook on h1.
        const string fen = "3k4/8/8/8/8/8/8/4K2R w K - 0 1";
        var board = new Board(fen);
        var expected = new Board(fen);

        // White castling kingside (e1→g1)
        board.MakeMove("e1g1");
        board.UndoMove();

        // After unmaking, the board should match the starting state.
        AssertBoardsEqual(expected, board);
    }

    [Fact]
    public void UnmakeMove_QueensideCastlingWhite_RestoresOriginalPosition()
    {
        // Set up a board with the white king on e1 and a white rook on a1.
        const string fen = "3k4/8/8/8/8/8/8/R3K3 w Q - 0 1";
        var board = new Board(fen);
        var expected = new Board(fen);

        // White queenside castling (e1→c1)
        board.MakeMove("e1c1");
        board.UndoMove();

        AssertBoardsEqual(expected, board);
    }

    [Fact]
    public void UnmakeMove_KingsideCastlingBlack_RestoresOriginalPosition()
    {
        // Set up a board with the black king on e8 and a black rook on h8.
        const string fen = "4k2r/8/8/8/8/8/8/4K3 b k - 0 1";
        var board = new Board(fen);
        var expected = new Board(fen);

        // Black kingside castling: e8 → g8.
        board.MakeMove("e8g8");
        board.UndoMove();

        AssertBoardsEqual(expected, board);
    }

    [Fact]
    public void UnmakeMove_QueensideCastlingBlack_RestoresOriginalPosition()
    {
        // Set up a board with the black king on e8 and a black rook on a8.
        const string fen = "r3k3/8/8/8/8/8/8/4K3 w q - 0 1";
        var board = new Board(fen);
        var expected = new Board(fen);

        // Black queenside castling: e8 → c8.
        board.MakeMove("e8c8");
        board.UndoMove();

        AssertBoardsEqual(expected, board);
    }

    [Fact]
    public void UnmakeMove_DoublePawnPushWhite_RestoresOriginalPosition()
    {
        // Using the standard starting position:
        // A double pawn push from e2 to e4 should set en passant to e3.
        var board = new Board();
        var expected = new Board();

        board.MakeMove("e2e4");
        board.UndoMove();

        AssertBoardsEqual(expected, board);
    }

    [Fact]
    public void UnmakeMove_DoublePawnPushBlack_RestoresOriginalPosition()
    {
        // In the standard starting position, white moves first.
        // After a dummy white move, a black pawn from e7 moves to e5,
        // which should set the en passant target to e6.
        var board = new Board();
        board.MakeMove("a2a3"); // White non-interfering move.

        // Capture the state after white's move as the expected state for black's move.
        var expected = new Board();
        expected.MakeMove("a2a3");

        board.MakeMove("e7e5");
        board.UndoMove();

        AssertBoardsEqual(expected, board);
    }

    [Theory]
    [InlineData("q", MoveType.PromoteToQueen)]
    [InlineData("r", MoveType.PromoteToRook)]
    [InlineData("b", MoveType.PromoteToBishop)]
    [InlineData("n", MoveType.PromoteToKnight)]
    public void UnmakeMove_PromotionWhite_RestoresOriginalPosition(string promo, MoveType expectedType)
    {
        // Create a position with a white pawn on g7 (ready to promote) and a black king.
        const string fen = "3k4/6P1/8/8/8/8/8/3K4 w - - 0 1";
        var board = new Board(fen);
        var expected = new Board(fen);

        var move = $"g7g8{promo}";
        board.MakeMove(move);
        board.UndoMove();

        AssertBoardsEqual(expected, board);
    }

    [Theory]
    [InlineData("q", MoveType.PromoteToQueen)]
    [InlineData("r", MoveType.PromoteToRook)]
    [InlineData("b", MoveType.PromoteToBishop)]
    [InlineData("n", MoveType.PromoteToKnight)]
    public void UnmakeMove_PromotionBlack_RestoresOriginalPosition(string promo, MoveType expectedType)
    {
        // Create a position with a black pawn on a2 (ready to promote) and a white king.
        const string fen = "3k4/8/8/8/8/8/p7/3K4 b - - 0 1";
        var board = new Board(fen);
        var expected = new Board(fen);

        var move = $"a2a1{promo}";
        board.MakeMove(move);
        board.UndoMove();

        AssertBoardsEqual(expected, board);
    }

    [Fact]
    public void UnmakeMove_EnPassantWhite_RestoresOriginalPosition()
    {
        // Set up a position where a white pawn on d5 can capture en passant a black pawn on e5.
        const string fen = "4k3/8/8/3Pp3/8/8/8/4K3 w - e6 0 1";
        var board = new Board(fen);
        var expected = new Board(fen);

        board.MakeMove("d5e6");
        board.UndoMove();

        AssertBoardsEqual(expected, board);
    }

    [Fact]
    public void UnmakeMove_EnPassantBlack_RestoresOriginalPosition()
    {
        // Set up a position where a black pawn on d4 can capture en passant a white pawn on e4.
        const string fen = "8/8/8/8/3pP3/8/8/8 b - e3 0 1";
        var board = new Board(fen);
        var expected = new Board(fen);

        board.MakeMove("d4e3");
        board.UndoMove();

        AssertBoardsEqual(expected, board);
    }
}
