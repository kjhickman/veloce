using ChessLite.Parsing;
using Veloce.Evaluation;

namespace Veloce.UnitTests;

public class MaterialEvaluatorTests
{
    [Test]
    public async Task Evaluate_WhenSideToMoveIsWinning_ReturnsPositiveScore()
    {
        var whiteToMove = Fen.Parse("4k3/8/8/8/8/8/8/Q3K3 w - - 0 1");
        var blackToMove = Fen.Parse("q3k3/8/8/8/8/8/8/4K3 b - - 0 1");

        await Assert.That(whiteToMove.Evaluate()).IsGreaterThan(0);
        await Assert.That(blackToMove.Evaluate()).IsGreaterThan(0);
    }

    [Test]
    public async Task Evaluate_InEndgame_PrefersCentralKing()
    {
        var centralKing = Fen.Parse("7k/8/8/8/4K3/8/8/8 w - - 0 1");
        var cornerKing = Fen.Parse("7k/8/8/8/8/8/8/K7 w - - 0 1");

        await Assert.That(centralKing.Evaluate()).IsGreaterThan(cornerKing.Evaluate());
    }

    [Test]
    public async Task Evaluate_InMiddlegame_PrefersSaferKing()
    {
        var saferKing = Fen.Parse("rnbqkbnr/8/8/8/8/8/8/RNBQ1RK1 w - - 0 1");
        var exposedKing = Fen.Parse("rnbqkbnr/8/8/8/4K3/8/8/RNBQ1R2 w - - 0 1");

        await Assert.That(saferKing.Evaluate()).IsGreaterThan(exposedKing.Evaluate());
    }

    [Test]
    public async Task Evaluate_InEndgame_PrefersAdvancedPawn()
    {
        var advancedPawn = Fen.Parse("7k/8/4P3/8/8/8/8/K7 w - - 0 1");
        var startingPawn = Fen.Parse("7k/8/8/8/8/8/4P3/K7 w - - 0 1");

        await Assert.That(advancedPawn.Evaluate()).IsGreaterThan(startingPawn.Evaluate());
    }

    [Test]
    public async Task Evaluate_InEndgame_PrefersActiveRook()
    {
        var activeRook = Fen.Parse("7k/R7/8/8/8/8/8/6K1 w - - 0 1");
        var passiveRook = Fen.Parse("7k/8/8/8/8/8/8/R5K1 w - - 0 1");

        await Assert.That(activeRook.Evaluate()).IsGreaterThan(passiveRook.Evaluate());
    }
}
