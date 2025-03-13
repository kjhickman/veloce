namespace Veloce.Tests.Engine.SearchTests;

public class TimeManagementTests
{
    [Fact]
    public void CalculateMoveTime_ShouldReturnCorrectMoveTime_WhenLowTime()
    {
        // Arrange
        var timeControl = new TimeControl(timeLeft: 500, increment: 1000);
        var expectedMoveTime = 150;

        // Act
        var moveTime = TimeManagement.CalculateMoveTime(timeControl);

        // Assert
        Assert.Equal(expectedMoveTime, moveTime);
    }

    [Fact]
    public void CalculateMoveTime_ShouldReturnCorrectMoveTime_WhenStarting()
    {
        // Arrange
        var timeControl = new TimeControl(timeLeft: 120_000, increment: 1000);
        var expectedMoveTime = 4870;

        // Act
        var moveTime = TimeManagement.CalculateMoveTime(timeControl);

        // Assert
        Assert.Equal(expectedMoveTime, moveTime);
    }
}
