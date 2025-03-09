using Shouldly;
using Zugfish.Engine;

namespace Zugfish.Tests.Engine.MoveGeneratorTests;

public class AttackGenerationTests
{
    [Theory]
    [InlineData("8/8/8/3R4/8/8/8/8 w - - 0 1", 14)]
    [InlineData("8/8/3r4/3R4/8/8/8/8 w - - 0 1", 12)]
    [InlineData("3k4/8/3r4/3R4/8/8/8/3K4 w - - 0 1", 12)]
    [InlineData("3k4/8/3r4/3R4/8/8/3K4/8 w - - 0 1", 11)]
    public void CalculateRookAttacks_ForWhite_ReturnsCorrectAttacks(string fen, int expected)
    {
        // Arrange
        var position = new Position(fen);
        var allPieces = position.AllPieces;

        // Act
        var whiteRookAttackCount = AttackGeneration.CalculateRookAttacks(position.WhiteRooks, allPieces).Count();

        // Assert
        whiteRookAttackCount.ShouldBe(expected);
    }

    [Theory]
    [InlineData("8/8/8/4r3/8/8/8/8 w - - 0 1", 14)]
    [InlineData("8/8/8/4r3/4R3/8/8/8 w - - 0 1", 11)]
    [InlineData("4k3/8/8/4r3/4R3/8/8/4K3 w - - 0 1", 11)]
    [InlineData("8/4k3/8/4r3/4R3/8/8/4K3 w - - 0 1", 10)]
    public void CalculateRookAttacks_ForBlack_ReturnsCorrectAttacks(string fen, int expected)
    {
        // Arrange
        var position = new Position(fen);
        var allPieces = position.AllPieces;

        // Act
        var whiteRookAttackCount = AttackGeneration.CalculateRookAttacks(position.BlackRooks, allPieces).Count();

        // Assert
        whiteRookAttackCount.ShouldBe(expected);
    }

    [Theory]
    [InlineData("8/8/8/4B3/8/8/8/8 w - - 0 1", 13)]
    [InlineData("2k5/8/8/3b4/4B3/8/8/3K4 w - - 0 1", 10)]
    [InlineData("2k5/8/8/3b4/4B3/3K4/8/8 w - - 0 1", 8)]
    public void CalculateBishopAttacks_ForWhite_ReturnsCorrectAttacks(string fen, int expected)
    {
        // Arrange
        var position = new Position(fen);
        var allPieces = position.AllPieces;

        // Act
        var whiteBishopAttackCount = AttackGeneration.CalculateBishopAttacks(position.WhiteBishops, allPieces).Count();

        // Assert
        whiteBishopAttackCount.ShouldBe(expected);
    }

    [Theory]
    [InlineData("8/8/8/3b4/8/8/8/8 w - - 0 1", 13)]
    [InlineData("2k5/8/8/3b4/4B3/8/8/3K4 w - - 0 1", 10)]
    [InlineData("8/8/2k5/3b4/4B3/8/8/3K4 w - - 0 1", 8)]
    public void CalculateBishopAttacks_ForBlack_ReturnsCorrectAttacks(string fen, int expected)
    {
        // Arrange
        var position = new Position(fen);
        var allPieces = position.AllPieces;

        // Act
        var whiteBishopAttackCount = AttackGeneration.CalculateBishopAttacks(position.BlackBishops, allPieces).Count();

        // Assert
        whiteBishopAttackCount.ShouldBe(expected);
    }

    [Theory]
    [InlineData("8/8/8/3Q4/8/8/8/8 w - - 0 1", 27)]
    [InlineData("8/1k6/3qB3/3Q4/8/8/3K4/8 w - - 0 1", 21)]
    public void CalculateQueenAttacks_ForWhite_ReturnsCorrectAttacks(string fen, int expected)
    {
        // Arrange
        var position = new Position(fen);
        var allPieces = position.AllPieces;

        // Act
        var whiteQueenAttackCount = AttackGeneration.CalculateQueenAttacks(position.WhiteQueens, allPieces).Count();

        // Assert
        whiteQueenAttackCount.ShouldBe(expected);
    }

    [Theory]
    [InlineData("8/8/8/3q4/8/8/8/8 w - - 0 1", 27)]
    [InlineData("8/1k6/3qB3/3Q4/8/8/3K4/8 w - - 0 1", 18)]
    public void CalculateQueenAttacks_ForBlack_ReturnsCorrectAttacks(string fen, int expected)
    {
        // Arrange
        var position = new Position(fen);
        var allPieces = position.AllPieces;

        // Act
        var whiteQueenAttackCount = AttackGeneration.CalculateQueenAttacks(position.BlackQueens, allPieces).Count();

        // Assert
        whiteQueenAttackCount.ShouldBe(expected);
    }
}
