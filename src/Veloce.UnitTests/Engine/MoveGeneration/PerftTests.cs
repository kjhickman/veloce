using Shouldly;
using Veloce.Core.Models;
using Veloce.State;
using static Veloce.Perft.Perft;

namespace Veloce.UnitTests.Engine.MoveGeneration;

public class PerftTests
{
    private readonly MoveExecutor _executor = new();

    // [Test]
    [Test, Skip("This test is only for debugging purposes")]
    public void Problem1()
    {
        var position = new Position("8/8/8/1Ppp3r/RK3p1k/8/4P1P1/8 w - c6 0 3");
        // var executor = new MoveExecutor();

        // Act
        Span<Move> moves = stackalloc Move[218];
        var moveCount = Movement.MoveGeneration.GenerateLegalMoves(position, moves);

        // Assert
        moveCount.ShouldBe(6);
    }
    
    [Test]
    [Arguments(1, 20)]
    [Arguments(2, 400)]
    [Arguments(3, 8902)]
    [Arguments(4, 197281)]
    [Arguments(5, 4865609)]
    [Arguments(6, 119060324)]
    public void Position1_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 48)]
    [Arguments(2, 2039)]
    [Arguments(3, 97862)]
    [Arguments(4, 4085603)]
    [Arguments(5, 193690690)]
    public void Position2_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 15)]
    [Arguments(2, 66)]
    [Arguments(3, 1197)]
    [Arguments(4, 7059)]
    [Arguments(5, 133987)]
    [Arguments(6, 764643)]
    public void Position3_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k3/8/8/8/8/8/8/4K2R w K - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 16)]
    [Arguments(2, 71)]
    [Arguments(3, 1287)]
    [Arguments(4, 7626)]
    [Arguments(5, 145232)]
    [Arguments(6, 846648)]
    public void Position4_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k3/8/8/8/8/8/8/R3K3 w Q - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 75)]
    [Arguments(3, 459)]
    [Arguments(4, 8290)]
    [Arguments(5, 47635)]
    [Arguments(6, 899442)]
    public void Position5_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k2r/8/8/8/8/8/8/4K3 w k - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 80)]
    [Arguments(3, 493)]
    [Arguments(4, 8897)]
    [Arguments(5, 52710)]
    [Arguments(6, 1001523)]
    public void Position6_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k3/8/8/8/8/8/8/4K3 w q - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 26)]
    [Arguments(2, 112)]
    [Arguments(3, 3189)]
    [Arguments(4, 17945)]
    [Arguments(5, 532933)]
    [Arguments(6, 2788982)]
    public void Position7_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k3/8/8/8/8/8/8/R3K2R w KQ - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 130)]
    [Arguments(3, 782)]
    [Arguments(4, 22180)]
    [Arguments(5, 118882)]
    [Arguments(6, 3517770)]
    public void Position8_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/4K3 w kq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 12)]
    [Arguments(2, 38)]
    [Arguments(3, 564)]
    [Arguments(4, 2219)]
    [Arguments(5, 37735)]
    [Arguments(6, 185867)]
    public void Position9_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/8/6k1/4K2R w K - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 15)]
    [Arguments(2, 65)]
    [Arguments(3, 1018)]
    [Arguments(4, 4573)]
    [Arguments(5, 80619)]
    [Arguments(6, 413018)]
    public void Position10_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/8/1k6/R3K3 w Q - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(2, 32)]
    [Arguments(3, 134)]
    [Arguments(4, 2073)]
    [Arguments(5, 10485)]
    [Arguments(6, 179869)]
    public void Position11_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k2r/6K1/8/8/8/8/8/8 w k - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 49)]
    [Arguments(3, 243)]
    [Arguments(4, 3991)]
    [Arguments(5, 20780)]
    [Arguments(6, 367724)]
    public void Position12_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k3/1K6/8/8/8/8/8/8 w q - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 26)]
    [Arguments(2, 568)]
    [Arguments(3, 13744)]
    [Arguments(4, 314346)]
    [Arguments(5, 7594526)]
    [Arguments(6, 179862938)]
    public void Position13_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 25)]
    [Arguments(2, 567)]
    [Arguments(3, 14095)]
    [Arguments(4, 328965)]
    [Arguments(5, 8153719)]
    [Arguments(6, 195629489)]
    public void Position14_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/1R2K2R w Kkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 25)]
    [Arguments(2, 548)]
    [Arguments(3, 13502)]
    [Arguments(4, 312835)]
    [Arguments(5, 7736373)]
    [Arguments(6, 184411439)]
    public void Position15_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/2R1K2R w Kkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 25)]
    [Arguments(2, 547)]
    [Arguments(3, 13579)]
    [Arguments(4, 316214)]
    [Arguments(5, 7878456)]
    [Arguments(6, 189224276)]
    public void Position16_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/R3K1R1 w Qkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 26)]
    [Arguments(2, 583)]
    [Arguments(3, 14252)]
    [Arguments(4, 334705)]
    [Arguments(5, 8198901)]
    [Arguments(6, 198328929)]
    public void Position17_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("1r2k2r/8/8/8/8/8/8/R3K2R w KQk - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 25)]
    [Arguments(2, 560)]
    [Arguments(3, 13592)]
    [Arguments(4, 317324)]
    [Arguments(5, 7710115)]
    [Arguments(6, 185959088)]
    public void Position18_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("2r1k2r/8/8/8/8/8/8/R3K2R w KQk - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 25)]
    [Arguments(2, 560)]
    [Arguments(3, 13607)]
    [Arguments(4, 320792)]
    [Arguments(5, 7848606)]
    [Arguments(6, 190755813)]
    public void Position19_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k1r1/8/8/8/8/8/8/R3K2R w KQq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 75)]
    [Arguments(3, 459)]
    [Arguments(4, 8290)]
    [Arguments(5, 47635)]
    [Arguments(6, 899442)]
    public void Position20_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k3/8/8/8/8/8/8/4K2R b K - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 80)]
    [Arguments(3, 493)]
    [Arguments(4, 8897)]
    [Arguments(5, 52710)]
    [Arguments(6, 1001523)]
    public void Position21_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k3/8/8/8/8/8/8/R3K3 b Q - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 15)]
    [Arguments(2, 66)]
    [Arguments(3, 1197)]
    [Arguments(4, 7059)]
    [Arguments(5, 133987)]
    [Arguments(6, 764643)]
    public void Position22_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k2r/8/8/8/8/8/8/4K3 b k - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 16)]
    [Arguments(2, 71)]
    [Arguments(3, 1287)]
    [Arguments(4, 7626)]
    [Arguments(5, 145232)]
    [Arguments(6, 846648)]
    public void Position23_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k3/8/8/8/8/8/8/4K3 b q - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 130)]
    [Arguments(3, 782)]
    [Arguments(4, 22180)]
    [Arguments(5, 118882)]
    [Arguments(6, 3517770)]
    public void Position24_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k3/8/8/8/8/8/8/R3K2R b KQ - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 26)]
    [Arguments(2, 112)]
    [Arguments(3, 3189)]
    [Arguments(4, 17945)]
    [Arguments(5, 532933)]
    [Arguments(6, 2788982)]
    public void Position25_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/4K3 b kq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(2, 32)]
    [Arguments(3, 134)]
    [Arguments(4, 2073)]
    [Arguments(5, 10485)]
    [Arguments(6, 179869)]
    public void Position26_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/8/6k1/4K2R b K - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 49)]
    [Arguments(3, 243)]
    [Arguments(4, 3991)]
    [Arguments(5, 20780)]
    [Arguments(6, 367724)]
    public void Position27_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/8/1k6/R3K3 b Q - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 12)]
    [Arguments(2, 38)]
    [Arguments(3, 564)]
    [Arguments(4, 2219)]
    [Arguments(5, 37735)]
    [Arguments(6, 185867)]
    public void Position28_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k2r/6K1/8/8/8/8/8/8 b k - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 15)]
    [Arguments(2, 65)]
    [Arguments(3, 1018)]
    [Arguments(4, 4573)]
    [Arguments(5, 80619)]
    [Arguments(6, 413018)]
    public void Position29_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k3/1K6/8/8/8/8/8/8 b q - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 26)]
    [Arguments(2, 568)]
    [Arguments(3, 13744)]
    [Arguments(4, 314346)]
    [Arguments(5, 7594526)]
    [Arguments(6, 179862938)]
    public void Position30_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 26)]
    [Arguments(2, 583)]
    [Arguments(3, 14252)]
    [Arguments(4, 334705)]
    [Arguments(5, 8198901)]
    [Arguments(6, 198328929)]
    public void Position31_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/1R2K2R b Kkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 25)]
    [Arguments(2, 560)]
    [Arguments(3, 13592)]
    [Arguments(4, 317324)]
    [Arguments(5, 7710115)]
    [Arguments(6, 185959088)]
    public void Position32_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/2R1K2R b Kkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 25)]
    [Arguments(2, 560)]
    [Arguments(3, 13607)]
    [Arguments(4, 320792)]
    [Arguments(5, 7848606)]
    [Arguments(6, 190755813)]
    public void Position33_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/R3K1R1 b Qkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 25)]
    [Arguments(2, 567)]
    [Arguments(3, 14095)]
    [Arguments(4, 328965)]
    [Arguments(5, 8153719)]
    [Arguments(6, 195629489)]
    public void Position34_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("1r2k2r/8/8/8/8/8/8/R3K2R b KQk - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 25)]
    [Arguments(2, 548)]
    [Arguments(3, 13502)]
    [Arguments(4, 312835)]
    [Arguments(5, 7736373)]
    [Arguments(6, 184411439)]
    public void Position35_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("2r1k2r/8/8/8/8/8/8/R3K2R b KQk - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 25)]
    [Arguments(2, 547)]
    [Arguments(3, 13579)]
    [Arguments(4, 316214)]
    [Arguments(5, 7878456)]
    [Arguments(6, 189224276)]
    public void Position36_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k1r1/8/8/8/8/8/8/R3K2R b KQq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 14)]
    [Arguments(2, 195)]
    [Arguments(3, 2760)]
    [Arguments(4, 38675)]
    [Arguments(5, 570726)]
    [Arguments(6, 8107539)]
    public void Position37_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/1n4N1/2k5/8/8/5K2/1N4n1/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 11)]
    [Arguments(2, 156)]
    [Arguments(3, 1636)]
    [Arguments(4, 20534)]
    [Arguments(5, 223507)]
    [Arguments(6, 2594412)]
    public void Position38_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/1k6/8/5N2/8/4n3/8/2K5 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 19)]
    [Arguments(2, 289)]
    [Arguments(3, 4442)]
    [Arguments(4, 73584)]
    [Arguments(5, 1198299)]
    [Arguments(6, 19870403)]
    public void Position39_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/4k3/3Nn3/3nN3/4K3/8/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(2, 51)]
    [Arguments(3, 345)]
    [Arguments(4, 5301)]
    [Arguments(5, 38348)]
    [Arguments(6, 588695)]
    public void Position40_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("K7/8/2n5/1n6/8/8/8/k6N w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 17)]
    [Arguments(2, 54)]
    [Arguments(3, 835)]
    [Arguments(4, 5910)]
    [Arguments(5, 92250)]
    [Arguments(6, 688780)]
    public void Position41_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/2N5/1N6/8/8/8/K6n w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 15)]
    [Arguments(2, 193)]
    [Arguments(3, 2816)]
    [Arguments(4, 40039)]
    [Arguments(5, 582642)]
    [Arguments(6, 8503277)]
    public void Position42_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/1n4N1/2k5/8/8/5K2/1N4n1/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 16)]
    [Arguments(2, 180)]
    [Arguments(3, 2290)]
    [Arguments(4, 24640)]
    [Arguments(5, 288141)]
    [Arguments(6, 3147566)]
    public void Position43_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/1k6/8/5N2/8/4n3/8/2K5 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 68)]
    [Arguments(3, 1118)]
    [Arguments(4, 16199)]
    [Arguments(5, 281190)]
    [Arguments(6, 4405103)]
    public void Position44_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/3K4/3Nn3/3nN3/4k3/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 17)]
    [Arguments(2, 54)]
    [Arguments(3, 835)]
    [Arguments(4, 5910)]
    [Arguments(5, 92250)]
    [Arguments(6, 688780)]
    public void Position45_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("K7/8/2n5/1n6/8/8/8/k6N b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(2, 51)]
    [Arguments(3, 345)]
    [Arguments(4, 5301)]
    [Arguments(5, 38348)]
    [Arguments(6, 588695)]
    public void Position46_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/2N5/1N6/8/8/8/K6n b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 17)]
    [Arguments(2, 278)]
    [Arguments(3, 4607)]
    [Arguments(4, 76778)]
    [Arguments(5, 1320507)]
    [Arguments(6, 22823890)]
    public void Position47_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("B6b/8/8/8/2K5/4k3/8/b6B w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 21)]
    [Arguments(2, 316)]
    [Arguments(3, 5744)]
    [Arguments(4, 93338)]
    [Arguments(5, 1713368)]
    [Arguments(6, 28861171)]
    public void Position48_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/1B6/7b/7k/8/2B1b3/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 21)]
    [Arguments(2, 144)]
    [Arguments(3, 3242)]
    [Arguments(4, 32955)]
    [Arguments(5, 787524)]
    [Arguments(6, 7881673)]
    public void Position49_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/B7/1B6/1B6/8/8/8/K6b w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 7)]
    [Arguments(2, 143)]
    [Arguments(3, 1416)]
    [Arguments(4, 31787)]
    [Arguments(5, 310862)]
    [Arguments(6, 7382896)]
    public void Position50_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("K7/b7/1b6/1b6/8/8/8/k6B w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 6)]
    [Arguments(2, 106)]
    [Arguments(3, 1829)]
    [Arguments(4, 31151)]
    [Arguments(5, 530585)]
    [Arguments(6, 9250746)]
    public void Position51_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("B6b/8/8/8/2K5/5k2/8/b6B b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 17)]
    [Arguments(2, 309)]
    [Arguments(3, 5133)]
    [Arguments(4, 93603)]
    [Arguments(5, 1591064)]
    [Arguments(6, 29027891)]
    public void Position52_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/1B6/7b/7k/8/2B1b3/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 7)]
    [Arguments(2, 143)]
    [Arguments(3, 1416)]
    [Arguments(4, 31787)]
    [Arguments(5, 310862)]
    [Arguments(6, 7382896)]
    public void Position53_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/B7/1B6/1B6/8/8/8/K6b b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 21)]
    [Arguments(2, 144)]
    [Arguments(3, 3242)]
    [Arguments(4, 32955)]
    [Arguments(5, 787524)]
    [Arguments(6, 7881673)]
    public void Position54_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("K7/b7/1b6/1b6/8/8/8/k6B b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 19)]
    [Arguments(2, 275)]
    [Arguments(3, 5300)]
    [Arguments(4, 104342)]
    [Arguments(5, 2161211)]
    [Arguments(6, 44956585)]
    public void Position55_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/RR6/8/8/8/8/rr6/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 36)]
    [Arguments(2, 1027)]
    [Arguments(3, 29215)]
    [Arguments(4, 771461)]
    [Arguments(5, 20506480)]
    [Arguments(6, 525169084)]
    public void Position56_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("R6r/8/8/2K5/5k2/8/8/r6R w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 19)]
    [Arguments(2, 275)]
    [Arguments(3, 5300)]
    [Arguments(4, 104342)]
    [Arguments(5, 2161211)]
    [Arguments(6, 44956585)]
    public void Position57_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/RR6/8/8/8/8/rr6/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 36)]
    [Arguments(2, 1027)]
    [Arguments(3, 29227)]
    [Arguments(4, 771368)]
    [Arguments(5, 20521342)]
    [Arguments(6, 524966748)]
    public void Position58_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("R6r/8/8/2K5/5k2/8/8/r6R b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 2)]
    [Arguments(2, 36)]
    [Arguments(3, 143)]
    [Arguments(4, 3637)]
    [Arguments(5, 14893)]
    [Arguments(6, 391507)]
    public void Position59_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("6kq/8/8/8/8/8/8/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 2)]
    [Arguments(2, 36)]
    [Arguments(3, 143)]
    [Arguments(4, 3637)]
    [Arguments(5, 14893)]
    [Arguments(6, 391507)]
    public void Position60_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("6KQ/8/8/8/8/8/8/7k b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 6)]
    [Arguments(2, 35)]
    [Arguments(3, 495)]
    [Arguments(4, 8349)]
    [Arguments(5, 166741)]
    [Arguments(6, 3370175)]
    public void Position61_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("K7/8/8/3Q4/4q3/8/8/7k w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 22)]
    [Arguments(2, 43)]
    [Arguments(3, 1015)]
    [Arguments(4, 4167)]
    [Arguments(5, 105749)]
    [Arguments(6, 419369)]
    public void Position62_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("6qk/8/8/8/8/8/8/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 2)]
    [Arguments(2, 36)]
    [Arguments(3, 143)]
    [Arguments(4, 3637)]
    [Arguments(5, 14893)]
    [Arguments(6, 391507)]
    public void Position63_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("6KQ/8/8/8/8/8/8/7k b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 6)]
    [Arguments(2, 35)]
    [Arguments(3, 495)]
    [Arguments(4, 8349)]
    [Arguments(5, 166741)]
    [Arguments(6, 3370175)]
    public void Position64_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("K7/8/8/3Q4/4q3/8/8/7k b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(2, 7)]
    [Arguments(3, 43)]
    [Arguments(4, 199)]
    [Arguments(5, 1347)]
    [Arguments(6, 6249)]
    public void Position65_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/K7/P7/k7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(2, 7)]
    [Arguments(3, 43)]
    [Arguments(4, 199)]
    [Arguments(5, 1347)]
    [Arguments(6, 6249)]
    public void Position66_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/7K/7P/7k w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 1)]
    [Arguments(2, 3)]
    [Arguments(3, 12)]
    [Arguments(4, 80)]
    [Arguments(5, 342)]
    [Arguments(6, 2343)]
    public void Position67_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("K7/p7/k7/8/8/8/8/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 1)]
    [Arguments(2, 3)]
    [Arguments(3, 12)]
    [Arguments(4, 80)]
    [Arguments(5, 342)]
    [Arguments(6, 2343)]
    public void Position68_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7K/7p/7k/8/8/8/8/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 7)]
    [Arguments(2, 35)]
    [Arguments(3, 210)]
    [Arguments(4, 1091)]
    [Arguments(5, 7028)]
    [Arguments(6, 34834)]
    public void Position69_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/2k1p3/3pP3/3P2K1/8/8/8/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 1)]
    [Arguments(2, 3)]
    [Arguments(3, 12)]
    [Arguments(4, 80)]
    [Arguments(5, 342)]
    [Arguments(6, 2343)]
    public void Position70_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/K7/P7/k7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 1)]
    [Arguments(2, 3)]
    [Arguments(3, 12)]
    [Arguments(4, 80)]
    [Arguments(5, 342)]
    [Arguments(6, 2343)]
    public void Position71_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/7K/7P/7k b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(2, 7)]
    [Arguments(3, 43)]
    [Arguments(4, 199)]
    [Arguments(5, 1347)]
    [Arguments(6, 6249)]
    public void Position72_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("K7/p7/k7/8/8/8/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(2, 7)]
    [Arguments(3, 43)]
    [Arguments(4, 199)]
    [Arguments(5, 1347)]
    [Arguments(6, 6249)]
    public void Position73_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7K/7p/7k/8/8/8/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 35)]
    [Arguments(3, 182)]
    [Arguments(4, 1091)]
    [Arguments(5, 5408)]
    [Arguments(6, 34822)]
    public void Position74_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/2k1p3/3pP3/3P2K1/8/8/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 2)]
    [Arguments(2, 8)]
    [Arguments(3, 44)]
    [Arguments(4, 282)]
    [Arguments(5, 1814)]
    [Arguments(6, 11848)]
    public void Position75_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/4k3/4P3/4K3 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 2)]
    [Arguments(2, 8)]
    [Arguments(3, 44)]
    [Arguments(4, 282)]
    [Arguments(5, 1814)]
    [Arguments(6, 11848)]
    public void Position76_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k3/4p3/4K3/8/8/8/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(2, 9)]
    [Arguments(3, 57)]
    [Arguments(4, 360)]
    [Arguments(5, 1969)]
    [Arguments(6, 10724)]
    public void Position77_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/7k/7p/7P/7K/8/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(2, 9)]
    [Arguments(3, 57)]
    [Arguments(4, 360)]
    [Arguments(5, 1969)]
    [Arguments(6, 10724)]
    public void Position78_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/k7/p7/P7/K7/8/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 25)]
    [Arguments(3, 180)]
    [Arguments(4, 1294)]
    [Arguments(5, 8296)]
    [Arguments(6, 53138)]
    public void Position79_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/3k4/3p4/3P4/3K4/8/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 8)]
    [Arguments(2, 61)]
    [Arguments(3, 483)]
    [Arguments(4, 3213)]
    [Arguments(5, 23599)]
    [Arguments(6, 157093)]
    public void Position80_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/3k4/3p4/8/3P4/3K4/8/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 8)]
    [Arguments(2, 61)]
    [Arguments(3, 411)]
    [Arguments(4, 3213)]
    [Arguments(5, 21637)]
    [Arguments(6, 158065)]
    public void Position81_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/3k4/3p4/8/3P4/3K4/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 15)]
    [Arguments(3, 90)]
    [Arguments(4, 534)]
    [Arguments(5, 3450)]
    [Arguments(6, 20960)]
    public void Position82_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/3p4/8/3P4/8/8/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(2, 9)]
    [Arguments(3, 57)]
    [Arguments(4, 360)]
    [Arguments(5, 1969)]
    [Arguments(6, 10724)]
    public void Position83_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/7k/7p/7P/7K/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(2, 9)]
    [Arguments(3, 57)]
    [Arguments(4, 360)]
    [Arguments(5, 1969)]
    [Arguments(6, 10724)]
    public void Position84_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/k7/p7/P7/K7/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 25)]
    [Arguments(3, 180)]
    [Arguments(4, 1294)]
    [Arguments(5, 8296)]
    [Arguments(6, 53138)]
    public void Position85_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/3k4/3p4/3P4/3K4/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 8)]
    [Arguments(2, 61)]
    [Arguments(3, 411)]
    [Arguments(4, 3213)]
    [Arguments(5, 21637)]
    [Arguments(6, 158065)]
    public void Position86_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/3k4/3p4/8/3P4/3K4/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 8)]
    [Arguments(2, 61)]
    [Arguments(3, 483)]
    [Arguments(4, 3213)]
    [Arguments(5, 23599)]
    [Arguments(6, 157093)]
    public void Position87_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/3k4/3p4/8/3P4/3K4/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 15)]
    [Arguments(3, 89)]
    [Arguments(4, 537)]
    [Arguments(5, 3309)]
    [Arguments(6, 21104)]
    public void Position88_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/3p4/8/3P4/8/8/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 19)]
    [Arguments(3, 117)]
    [Arguments(4, 720)]
    [Arguments(5, 4661)]
    [Arguments(6, 32191)]
    public void Position89_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/3p4/8/8/3P4/8/8/K7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 19)]
    [Arguments(3, 116)]
    [Arguments(4, 716)]
    [Arguments(5, 4786)]
    [Arguments(6, 30980)]
    public void Position90_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/8/3p4/8/8/3P4/K7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 22)]
    [Arguments(3, 139)]
    [Arguments(4, 877)]
    [Arguments(5, 6112)]
    [Arguments(6, 41874)]
    public void Position91_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/8/7p/6P1/8/8/K7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 16)]
    [Arguments(3, 101)]
    [Arguments(4, 637)]
    [Arguments(5, 4354)]
    [Arguments(6, 29679)]
    public void Position92_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/7p/8/8/6P1/8/K7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 22)]
    [Arguments(3, 139)]
    [Arguments(4, 877)]
    [Arguments(5, 6112)]
    [Arguments(6, 41874)]
    public void Position93_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/8/6p1/7P/8/8/K7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 16)]
    [Arguments(3, 101)]
    [Arguments(4, 637)]
    [Arguments(5, 4354)]
    [Arguments(6, 29679)]
    public void Position94_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/6p1/8/8/7P/8/K7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 3)]
    [Arguments(2, 15)]
    [Arguments(3, 84)]
    [Arguments(4, 573)]
    [Arguments(5, 3013)]
    [Arguments(6, 22886)]
    public void Position95_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/8/3p4/4p3/8/8/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 16)]
    [Arguments(3, 101)]
    [Arguments(4, 637)]
    [Arguments(5, 4271)]
    [Arguments(6, 28662)]
    public void Position96_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/3p4/8/8/4P3/8/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 19)]
    [Arguments(3, 117)]
    [Arguments(4, 720)]
    [Arguments(5, 5014)]
    [Arguments(6, 32167)]
    public void Position97_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/3p4/8/8/3P4/8/8/K7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 19)]
    [Arguments(3, 117)]
    [Arguments(4, 712)]
    [Arguments(5, 4658)]
    [Arguments(6, 30749)]
    public void Position98_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/8/3p4/8/8/3P4/K7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 22)]
    [Arguments(3, 139)]
    [Arguments(4, 877)]
    [Arguments(5, 6112)]
    [Arguments(6, 41874)]
    public void Position99_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/8/7p/6P1/8/8/K7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 16)]
    [Arguments(3, 101)]
    [Arguments(4, 637)]
    [Arguments(5, 4354)]
    [Arguments(6, 29679)]
    public void Position100_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/7p/8/8/6P1/8/K7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 22)]
    [Arguments(3, 139)]
    [Arguments(4, 877)]
    [Arguments(5, 6112)]
    [Arguments(6, 41874)]
    public void Position101_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/8/6p1/7P/8/8/K7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 16)]
    [Arguments(3, 101)]
    [Arguments(4, 637)]
    [Arguments(5, 4354)]
    [Arguments(6, 29679)]
    public void Position102_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/6p1/8/8/7P/8/K7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 15)]
    [Arguments(3, 102)]
    [Arguments(4, 569)]
    [Arguments(5, 4337)]
    [Arguments(6, 22579)]
    public void Position103_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/8/3p4/4p3/8/8/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 16)]
    [Arguments(3, 101)]
    [Arguments(4, 637)]
    [Arguments(5, 4271)]
    [Arguments(6, 28662)]
    public void Position104_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/3p4/8/8/4P3/8/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 22)]
    [Arguments(3, 139)]
    [Arguments(4, 877)]
    [Arguments(5, 6112)]
    [Arguments(6, 41874)]
    public void Position105_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/8/p7/1P6/8/8/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 16)]
    [Arguments(3, 101)]
    [Arguments(4, 637)]
    [Arguments(5, 4354)]
    [Arguments(6, 29679)]
    public void Position106_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/p7/8/8/1P6/8/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 22)]
    [Arguments(3, 139)]
    [Arguments(4, 877)]
    [Arguments(5, 6112)]
    [Arguments(6, 41874)]
    public void Position107_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/8/1p6/P7/8/8/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 16)]
    [Arguments(3, 101)]
    [Arguments(4, 637)]
    [Arguments(5, 4354)]
    [Arguments(6, 29679)]
    public void Position108_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/1p6/8/8/P7/8/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 25)]
    [Arguments(3, 161)]
    [Arguments(4, 1035)]
    [Arguments(5, 7574)]
    [Arguments(6, 55338)]
    public void Position109_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/7p/8/8/8/8/6P1/K7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 25)]
    [Arguments(3, 161)]
    [Arguments(4, 1035)]
    [Arguments(5, 7574)]
    [Arguments(6, 55338)]
    public void Position110_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/6p1/8/8/8/8/7P/K7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 7)]
    [Arguments(2, 49)]
    [Arguments(3, 378)]
    [Arguments(4, 2902)]
    [Arguments(5, 24122)]
    [Arguments(6, 199002)]
    public void Position111_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("3k4/3pp3/8/8/8/8/3PP3/3K4 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 22)]
    [Arguments(3, 139)]
    [Arguments(4, 877)]
    [Arguments(5, 6112)]
    [Arguments(6, 41874)]
    public void Position112_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/8/p7/1P6/8/8/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 16)]
    [Arguments(3, 101)]
    [Arguments(4, 637)]
    [Arguments(5, 4354)]
    [Arguments(6, 29679)]
    public void Position113_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/p7/8/8/1P6/8/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 22)]
    [Arguments(3, 139)]
    [Arguments(4, 877)]
    [Arguments(5, 6112)]
    [Arguments(6, 41874)]
    public void Position114_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/8/1p6/P7/8/8/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 4)]
    [Arguments(2, 16)]
    [Arguments(3, 101)]
    [Arguments(4, 637)]
    [Arguments(5, 4354)]
    [Arguments(6, 29679)]
    public void Position115_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/1p6/8/8/P7/8/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 25)]
    [Arguments(3, 161)]
    [Arguments(4, 1035)]
    [Arguments(5, 7574)]
    [Arguments(6, 55338)]
    public void Position116_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/7p/8/8/8/8/6P1/K7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 5)]
    [Arguments(2, 25)]
    [Arguments(3, 161)]
    [Arguments(4, 1035)]
    [Arguments(5, 7574)]
    [Arguments(6, 55338)]
    public void Position117_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/6p1/8/8/8/8/7P/K7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 7)]
    [Arguments(2, 49)]
    [Arguments(3, 378)]
    [Arguments(4, 2902)]
    [Arguments(5, 24122)]
    [Arguments(6, 199002)]
    public void Position118_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("3k4/3pp3/8/8/8/8/3PP3/3K4 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 11)]
    [Arguments(2, 97)]
    [Arguments(3, 887)]
    [Arguments(4, 8048)]
    [Arguments(5, 90606)]
    [Arguments(6, 1030499)]
    public void Position119_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/Pk6/8/8/8/8/6Kp/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 24)]
    [Arguments(2, 421)]
    [Arguments(3, 7421)]
    [Arguments(4, 124608)]
    [Arguments(5, 2193768)]
    [Arguments(6, 37665329)]
    public void Position120_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("n1n5/1Pk5/8/8/8/8/5Kp1/5N1N w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 18)]
    [Arguments(2, 270)]
    [Arguments(3, 4699)]
    [Arguments(4, 79355)]
    [Arguments(5, 1533145)]
    [Arguments(6, 28859283)]
    public void Position121_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/PPPk4/8/8/8/8/4Kppp/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 24)]
    [Arguments(2, 496)]
    [Arguments(3, 9483)]
    [Arguments(4, 182838)]
    [Arguments(5, 3605103)]
    [Arguments(6, 71179139)]
    public void Position122_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("n1n5/PPPk4/8/8/8/8/4Kppp/5N1N w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 11)]
    [Arguments(2, 97)]
    [Arguments(3, 887)]
    [Arguments(4, 8048)]
    [Arguments(5, 90606)]
    [Arguments(6, 1030499)]
    public void Position123_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/Pk6/8/8/8/8/6Kp/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 24)]
    [Arguments(2, 421)]
    [Arguments(3, 7421)]
    [Arguments(4, 124608)]
    [Arguments(5, 2193768)]
    [Arguments(6, 37665329)]
    public void Position124_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("n1n5/1Pk5/8/8/8/8/5Kp1/5N1N b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 18)]
    [Arguments(2, 270)]
    [Arguments(3, 4699)]
    [Arguments(4, 79355)]
    [Arguments(5, 1533145)]
    [Arguments(6, 28859283)]
    public void Position125_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/PPPk4/8/8/8/8/4Kppp/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(1, 24)]
    [Arguments(2, 496)]
    [Arguments(3, 9483)]
    [Arguments(4, 182838)]
    [Arguments(5, 3605103)]
    [Arguments(6, 71179139)]
    public void Position126_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("n1n5/PPPk4/8/8/8/8/4Kppp/5N1N b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(4, 43238)]
    [Arguments(5, 674624)]
    [Arguments(6, 11030083)]
    public void Position127_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Test]
    [Arguments(5, 11139762)]
    public void Position128_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("rnbqkb1r/ppppp1pp/7n/4Pp2/8/8/PPPP1PPP/RNBQKBNR w KQkq f6 0 3");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

}
