using Shouldly;
using Veloce.Core.Models;
using Veloce.State;

namespace Veloce.UnitTests.Engine.MoveGeneration;

public class GenerateLegalMovesTests
{
    [Test]
    public void GenerateLegalMoves_InitialPosition_Returns20Moves()
    {
        // Arrange
        var position = new Position();

        // Act
        Span<Move> moves = stackalloc Move[218];
        var moveCount = Movement.MoveGeneration.GenerateLegalMoves(position, moves);

        // Assert
        moveCount.ShouldBe(20);
    }

    [Test]
    public void GenerateLegalMoves_PetrovicPosition_Returns218Moves()
    {
        // Arrange
        var position = new Position("R6R/3Q4/1Q4Q1/4Q3/2Q4Q/Q4Q2/pp1Q4/kBNN1KB1 w - - 0 1");

        // Act
        Span<Move> moves = stackalloc Move[218];
        var moveCount = Movement.MoveGeneration.GenerateLegalMoves(position, moves);

        // Assert
        moveCount.ShouldBe(218);
    }

    [Test]
    public void GenerateLegalMoves_WhenCastlingWouldBeIntoCheck_CannotCastle()
    {
        // Arrange
        var position = new Position("r1bqk2r/ppp2ppp/2np1n2/2b1p3/2B1PP2/2N2N2/PPPP2PP/R1BQK2R w KQkq - 2 6");

        // Act
        Span<Move> moves = stackalloc Move[218];
        var moveCount = Movement.MoveGeneration.GenerateLegalMoves(position, moves);

        // Assert
        moveCount.ShouldBe(37);
        moves.ToArray().ShouldNotContain(x => x.SpecialMoveType == SpecialMoveType.ShortCastle);
    }

    [Test]
    public void GenerateLegalMoves_WhenInCheckAsWhite_CannotCastle()
    {
        // Arrange
        var position = new Position("r1bqkbnr/pppp1ppp/8/4p3/2B1P3/3P1N2/PPn2PPP/RNBQK2R w KQkq - 0 5");

        // Act
        Span<Move> moves = stackalloc Move[218];
        var moveCount = Movement.MoveGeneration.GenerateLegalMoves(position, moves);

        // Assert
        moveCount.ShouldBe(4);
        moves.ToArray().ShouldNotContain(x => x.SpecialMoveType == SpecialMoveType.ShortCastle);
    }

    [Test]
    public void GenerateLegalMoves_WhenPieceIsPinned_PieceCannotMove()
    {
        // Arrange
        var position = new Position("r1bqk1nr/pppp1ppp/2n5/4p3/1b2P3/2NP4/PPP2PPP/R1BQKBNR w KQkq - 3 4");

        // Act
        Span<Move> moves = stackalloc Move[218];
        Movement.MoveGeneration.GenerateLegalMoves(position, moves);

        // Assert
        moves.ToArray().ShouldNotContain(x => x.From == Square.c3);
    }

    [Test]
    public void GenerateLegalMoves_WhenEnPassantAvailable_FindsEnPassantMove()
    {
        // Arrange
        var position = new Position("4k3/8/8/3Pp3/8/8/8/4K3 w - e6 0 1");

        // Act
        Span<Move> moves = stackalloc Move[218];
        var moveCount = Movement.MoveGeneration.GenerateLegalMoves(position, moves);

        // Assert
        moveCount.ShouldBe(7);
        moves.ToArray().ShouldContain(x => x.SpecialMoveType == SpecialMoveType.EnPassant);
    }
}
