using Zugfish.Engine;
using Zugfish.UCI;
using static Zugfish.Engine.Translation;

namespace Zugfish.Tests.Engine.BoardTests;

public class MakeMoveTests
{
    [Fact]
    public void MakeMove_CastlingKingsideWhite_UpdatesKingAndRookPositions()
    {
        // Set up a board with only the white king on e1 and a white rook on h1
        const string fen = "3k4/8/8/8/8/8/8/4K2R w K - 0 1";
        var board = new Board(fen);

        // White castling kingside (e1→g1)
        board.MakeMove("e1g1");

        // After castling, the king should be on g1 and the rook on f1
        Assert.Equal(1UL << SquareFromAlgebraic("g1"), board.WhiteKing.Value);
        Assert.Equal(1UL << SquareFromAlgebraic("f1"), board.WhiteRooks.Value);
        Assert.Equal(0, board.CastlingRights);
    }

    [Fact]
    public void MakeMove_CastlingQueensideWhite_UpdatesKingAndRookPositions()
    {
        // Set up a board with the white king on e1 and a white rook on a1
        const string fen = "3k4/8/8/8/8/8/8/R3K3 w Q - 0 1";
        var board = new Board(fen);

        // White queenside castling (e1→c1)
        board.MakeMove("e1c1");

        // After castling, the king should be on c1 and the rook should move from a1 to d1
        Assert.Equal(1UL << SquareFromAlgebraic("c1"), board.WhiteKing.Value);
        Assert.Equal(1UL << SquareFromAlgebraic("d1"), board.WhiteRooks.Value);
        Assert.Equal(0, board.CastlingRights);
    }

    [Fact]
    public void MakeMove_CastlingKingsideBlack_UpdatesKingAndRookPositions()
    {
        // Set up a board with the black king on e8 and a black rook on h8.
        var fen = "4k2r/8/8/8/8/8/8/4K3 b k - 0 1";
        var board = new Board(fen);

        // Black kingside castling: e8 -> g8.
        board.MakeMove("e8g8");

        // After castling, the king should be on g8 and the rook on f8.
        Assert.Equal(1UL << SquareFromAlgebraic("g8"), board.BlackKing.Value);
        Assert.Equal(1UL << SquareFromAlgebraic("f8"), board.BlackRooks.Value);
        Assert.Equal(0, board.CastlingRights);
    }

    [Fact]
    public void MakeMove_CastlingQueensideBlack_UpdatesKingAndRookPositions()
    {
        // Set up a board with the black king on e8 and a black rook on a8.
        var fen = "r3k3/8/8/8/8/8/8/4K3 w q - 0 1";
        var board = new Board(fen);

        // Black queenside castling: e8 -> c8.
        board.MakeMove("e8c8");

        // After castling, the king should be on c8 and the rook on d8.
        Assert.Equal(1UL << SquareFromAlgebraic("c8"), board.BlackKing.Value);
        Assert.Equal(1UL << SquareFromAlgebraic("d8"), board.BlackRooks.Value);
        Assert.Equal(0, board.CastlingRights);
    }

    [Fact]
    public void MakeMove_DoublePawnPushWhite_SetsEnPassantTarget()
    {
        // Using the standard starting position:
        // A double pawn push from e2 to e4 should set the en passant target to e3.
        var board = new Board();
        board.MakeMove("e2e4");
        Assert.False((board.WhitePawns & (1UL << SquareFromAlgebraic("e2"))) != 0); // Pawn left e2.
        Assert.True((board.WhitePawns & (1UL << SquareFromAlgebraic("e4"))) != 0);  // Pawn appears on e4.
        Assert.Equal(SquareFromAlgebraic("e3"), board.EnPassantTarget);
    }

    [Fact]
    public void MakeMove_DoublePawnPushBlack_SetsEnPassantTarget()
    {
        // In the standard starting position white moves first.
        // After a dummy white move, black pawn from e7 can move to e5.
        // The double pawn push should set the en passant target to e6.
        var board = new Board();
        board.MakeMove("a2a3"); // White makes a non-interfering move.
        board.MakeMove("e7e5");
        Assert.False((board.BlackPawns & (1UL << SquareFromAlgebraic("e7"))) != 0);
        Assert.True((board.BlackPawns & (1UL << SquareFromAlgebraic("e5"))) != 0);
        Assert.Equal(SquareFromAlgebraic("e6"), board.EnPassantTarget);
    }

    [Theory]
    [InlineData("q", MoveType.PromoteToQueen)]
    [InlineData("r", MoveType.PromoteToRook)]
    [InlineData("b", MoveType.PromoteToBishop)]
    [InlineData("n", MoveType.PromoteToKnight)]
    public void MakeMove_PromotionWhite_UpdatesPiece(string promo, MoveType expectedType)
    {
        // Create a position with a white pawn on g7 (ready to promote) and a black king.
        const string fen = "3k4/6P1/8/8/8/8/8/3K4 w - - 0 1";
        var board = new Board(fen);
        var move = $"g7g8{promo}";
        board.MakeMove(move);

        // The pawn should be removed from g7 and the promoted piece placed on g8.
        Assert.False((board.WhitePawns & (1UL << SquareFromAlgebraic("g7"))) != 0);

        switch (expectedType)
        {
            case MoveType.PromoteToQueen:
                Assert.True((board.WhiteQueens & (1UL << SquareFromAlgebraic("g8"))) != 0, "White queen should be on g8");
                break;
            case MoveType.PromoteToRook:
                Assert.True((board.WhiteRooks & (1UL << SquareFromAlgebraic("g8"))) != 0, "White rook should be on g8");
                break;
            case MoveType.PromoteToBishop:
                Assert.True((board.WhiteBishops & (1UL << SquareFromAlgebraic("g8"))) != 0, "White bishop should be on g8");
                break;
            case MoveType.PromoteToKnight:
                Assert.True((board.WhiteKnights & (1UL << SquareFromAlgebraic("g8"))) != 0, "White knight should be on g8");
                break;
        }
    }

    [Theory]
    [InlineData("q", MoveType.PromoteToQueen)]
    [InlineData("r", MoveType.PromoteToRook)]
    [InlineData("b", MoveType.PromoteToBishop)]
    [InlineData("n", MoveType.PromoteToKnight)]
    public void MakeMove_PromotionBlack_UpdatesPiece(string promo, MoveType expectedType)
    {
        // Create a position with a black pawn on a2 (ready to promote) and a white king.
        const string fen = "3k4/8/8/8/8/8/p7/3K4 b - - 0 1";
        var board = new Board(fen);
        var move = $"a2a1{promo}";
        board.MakeMove(move);

        // The pawn should be removed from a2 and the promoted piece placed on a1.
        Assert.False((board.BlackPawns & (1UL << SquareFromAlgebraic("a2"))) != 0);

        switch (expectedType)
        {
            case MoveType.PromoteToQueen:
                Assert.True((board.BlackQueens & (1UL << SquareFromAlgebraic("a1"))) != 0, "Black queen should be on a1");
                break;
            case MoveType.PromoteToRook:
                Assert.True((board.BlackRooks & (1UL << SquareFromAlgebraic("a1"))) != 0, "Black rook should be on a1");
                break;
            case MoveType.PromoteToBishop:
                Assert.True((board.BlackBishops & (1UL << SquareFromAlgebraic("a1"))) != 0, "Black bishop should be on a1");
                break;
            case MoveType.PromoteToKnight:
                Assert.True((board.BlackKnights & (1UL << SquareFromAlgebraic("a1"))) != 0, "Black knight should be on a1");
                break;
        }
    }

    [Fact]
    public void MakeMove_EnPassantWhite_CapturesOpponentPawn()
    {
        // Set up a position where a white pawn on d5 can capture en passant a black pawn on e5
        const string fen = "4k3/8/8/3Pp3/8/8/8/4K3 w - e6 0 1";
        var board = new Board(fen);

        // White en passant move: d5 -> e6
        board.MakeMove("d5e6");

        // After en passant, the white pawn should now be on e6
        // and the black pawn on e5 should be captured.
        Assert.True((board.WhitePawns & (1UL << SquareFromAlgebraic("e6"))) != 0, "White pawn should be on e6 after en passant");
        Assert.False((board.BlackPawns & (1UL << SquareFromAlgebraic("e5"))) != 0, "Black pawn on e5 should be captured via en passant");
    }

    [Fact]
    public void MakeMove_EnPassantBlack_CapturesOpponentPawn()
    {
        // Set up a position where a black pawn on d4 can capture en passant a white pawn on e4.
        const string fen = "8/8/8/8/3pP3/8/8/8 b - e3 0 1";
        var board = new Board(fen);

        // Black en passant move: d4 -> e3.
        board.MakeMove("d4e3");

        // After en passant, the black pawn should now be on e3
        // and the white pawn on e4 should be captured.
        Assert.True((board.BlackPawns & (1UL << SquareFromAlgebraic("e3"))) != 0, "Black pawn should be on e3 after en passant");
        Assert.False((board.WhitePawns & (1UL << SquareFromAlgebraic("e4"))) != 0, "White pawn on e4 should be captured via en passant");
    }
}
