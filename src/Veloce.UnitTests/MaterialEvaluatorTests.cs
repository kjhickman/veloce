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
}
