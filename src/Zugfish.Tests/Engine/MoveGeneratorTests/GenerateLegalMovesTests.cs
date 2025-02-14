using Zugfish.Engine;
using Zugfish.Engine.Models;
using static Zugfish.Engine.Translation;

namespace Zugfish.Tests.Engine.MoveGeneratorTests;

public class GenerateLegalMovesTests
{
    [Fact]
    public void GenerateLegalMoves_InitialPosition_Returns20Moves()
    {
        // Arrange
        var position = new Position();
        var moveGenerator = new MoveGenerator();

        // Act
        Span<Move> moves = stackalloc Move[218];
        var moveCount = moveGenerator.GenerateLegalMoves(position, moves);

        // Assert
        Assert.Equal(20, moveCount);
    }

    [Fact]
    public void GenerateLegalMoves_PetrovicPosition_Returns218Moves()
    {
        // Arrange
        var position = new Position("R6R/3Q4/1Q4Q1/4Q3/2Q4Q/Q4Q2/pp1Q4/kBNN1KB1 w - - 0 1");
        var moveGenerator = new MoveGenerator();

        // Act
        Span<Move> moves = stackalloc Move[218];
        var moveCount = moveGenerator.GenerateLegalMoves(position, moves);

        // Assert
        Assert.Equal(218, moveCount);
    }

    [Fact]
    public void GenerateLegalMoves_WhenCastlingWouldBeIntoCheck_CannotCastle()
    {
        // Arrange
        var position = new Position("r1bqk2r/ppp2ppp/2np1n2/2b1p3/2B1PP2/2N2N2/PPPP2PP/R1BQK2R w KQkq - 2 6");
        var moveGenerator = new MoveGenerator();

        // Act
        Span<Move> moves = stackalloc Move[218];
        var moveCount = moveGenerator.GenerateLegalMoves(position, moves);

        // Assert
        var condition = moves.Contains(new Move(SquareFromAlgebraic("e1"), SquareFromAlgebraic("g1"), MoveType.Castling));
        Assert.False(condition, "White cannot castle into check");
        Assert.Equal(37, moveCount);
    }

    [Fact]
    public void GenerateLegalMoves_WhenInCheckAsWhite_CannotCastle()
    {
        // Arrange
        var position = new Position("r1bqkbnr/pppp1ppp/8/4p3/2B1P3/3P1N2/PPn2PPP/RNBQK2R w KQkq - 0 5");
        var moveGenerator = new MoveGenerator();

        // Act
        Span<Move> moves = stackalloc Move[218];
        var moveCount = moveGenerator.GenerateLegalMoves(position, moves);

        // Assert
        var condition = moves.Contains(new Move(SquareFromAlgebraic("e1"), SquareFromAlgebraic("g1"), MoveType.Castling));
        Assert.False(condition, "White cannot castle when in check");
        Assert.Equal(4, moveCount);
    }

    [Fact]
    public void GenerateLegalMoves_WhenPieceIsPinned_PieceCannotMove()
    {
        // Arrange
        var position = new Position("r1bqk1nr/pppp1ppp/2n5/4p3/1b2P3/2NP4/PPP2PPP/R1BQKBNR w KQkq - 3 4");
        var moveGenerator = new MoveGenerator();

        // Act
        Span<Move> moves = stackalloc Move[218];
        var moveCount = moveGenerator.GenerateLegalMoves(position, moves);

        // Assert
        var condition = moves[..moveCount].ToArray().Any(x => x.From == SquareFromAlgebraic("c3"));
        Assert.False(condition, "Pinned knight on c3 should not be able to move");
    }
}
