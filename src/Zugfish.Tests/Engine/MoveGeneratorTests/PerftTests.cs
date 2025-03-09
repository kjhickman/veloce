using Shouldly;
using Zugfish.Engine;
using Zugfish.Engine.Models;
using static Zugfish.Perft.Perft;

namespace Zugfish.Tests.Engine.MoveGeneratorTests;

public class PerftTests
{
    private readonly MoveExecutor _executor = new();

    // [Fact]
    [Fact(Skip = "This test is only for debugging purposes")]
    public void Problem1()
    {
        var position = new Position("8/8/8/1Ppp3r/RK3p1k/8/4P1P1/8 w - c6 0 3");
        // var executor = new MoveExecutor();

        // Act
        Span<Move> moves = stackalloc Move[218];
        var moveCount = MoveGeneration.GenerateLegalMoves(position, moves);

        // Assert
        moveCount.ShouldBe(6);
    }
    
    [Theory]
    [InlineData(1, 20)]
    [InlineData(2, 400)]
    [InlineData(3, 8902)]
    [InlineData(4, 197281)]
    [InlineData(5, 4865609)]
    [InlineData(6, 119060324)]
    public void Position1_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 48)]
    [InlineData(2, 2039)]
    [InlineData(3, 97862)]
    [InlineData(4, 4085603)]
    [InlineData(5, 193690690)]
    public void Position2_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 15)]
    [InlineData(2, 66)]
    [InlineData(3, 1197)]
    [InlineData(4, 7059)]
    [InlineData(5, 133987)]
    [InlineData(6, 764643)]
    public void Position3_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k3/8/8/8/8/8/8/4K2R w K - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 16)]
    [InlineData(2, 71)]
    [InlineData(3, 1287)]
    [InlineData(4, 7626)]
    [InlineData(5, 145232)]
    [InlineData(6, 846648)]
    public void Position4_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k3/8/8/8/8/8/8/R3K3 w Q - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 75)]
    [InlineData(3, 459)]
    [InlineData(4, 8290)]
    [InlineData(5, 47635)]
    [InlineData(6, 899442)]
    public void Position5_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k2r/8/8/8/8/8/8/4K3 w k - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 80)]
    [InlineData(3, 493)]
    [InlineData(4, 8897)]
    [InlineData(5, 52710)]
    [InlineData(6, 1001523)]
    public void Position6_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k3/8/8/8/8/8/8/4K3 w q - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 26)]
    [InlineData(2, 112)]
    [InlineData(3, 3189)]
    [InlineData(4, 17945)]
    [InlineData(5, 532933)]
    [InlineData(6, 2788982)]
    public void Position7_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k3/8/8/8/8/8/8/R3K2R w KQ - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 130)]
    [InlineData(3, 782)]
    [InlineData(4, 22180)]
    [InlineData(5, 118882)]
    [InlineData(6, 3517770)]
    public void Position8_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/4K3 w kq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 12)]
    [InlineData(2, 38)]
    [InlineData(3, 564)]
    [InlineData(4, 2219)]
    [InlineData(5, 37735)]
    [InlineData(6, 185867)]
    public void Position9_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/8/6k1/4K2R w K - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 15)]
    [InlineData(2, 65)]
    [InlineData(3, 1018)]
    [InlineData(4, 4573)]
    [InlineData(5, 80619)]
    [InlineData(6, 413018)]
    public void Position10_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/8/1k6/R3K3 w Q - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 32)]
    [InlineData(3, 134)]
    [InlineData(4, 2073)]
    [InlineData(5, 10485)]
    [InlineData(6, 179869)]
    public void Position11_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k2r/6K1/8/8/8/8/8/8 w k - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 49)]
    [InlineData(3, 243)]
    [InlineData(4, 3991)]
    [InlineData(5, 20780)]
    [InlineData(6, 367724)]
    public void Position12_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k3/1K6/8/8/8/8/8/8 w q - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 26)]
    [InlineData(2, 568)]
    [InlineData(3, 13744)]
    [InlineData(4, 314346)]
    [InlineData(5, 7594526)]
    [InlineData(6, 179862938)]
    public void Position13_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/R3K2R w KQkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 25)]
    [InlineData(2, 567)]
    [InlineData(3, 14095)]
    [InlineData(4, 328965)]
    [InlineData(5, 8153719)]
    [InlineData(6, 195629489)]
    public void Position14_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/1R2K2R w Kkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 25)]
    [InlineData(2, 548)]
    [InlineData(3, 13502)]
    [InlineData(4, 312835)]
    [InlineData(5, 7736373)]
    [InlineData(6, 184411439)]
    public void Position15_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/2R1K2R w Kkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 25)]
    [InlineData(2, 547)]
    [InlineData(3, 13579)]
    [InlineData(4, 316214)]
    [InlineData(5, 7878456)]
    [InlineData(6, 189224276)]
    public void Position16_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/R3K1R1 w Qkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 26)]
    [InlineData(2, 583)]
    [InlineData(3, 14252)]
    [InlineData(4, 334705)]
    [InlineData(5, 8198901)]
    [InlineData(6, 198328929)]
    public void Position17_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("1r2k2r/8/8/8/8/8/8/R3K2R w KQk - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 25)]
    [InlineData(2, 560)]
    [InlineData(3, 13592)]
    [InlineData(4, 317324)]
    [InlineData(5, 7710115)]
    [InlineData(6, 185959088)]
    public void Position18_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("2r1k2r/8/8/8/8/8/8/R3K2R w KQk - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 25)]
    [InlineData(2, 560)]
    [InlineData(3, 13607)]
    [InlineData(4, 320792)]
    [InlineData(5, 7848606)]
    [InlineData(6, 190755813)]
    public void Position19_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k1r1/8/8/8/8/8/8/R3K2R w KQq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 75)]
    [InlineData(3, 459)]
    [InlineData(4, 8290)]
    [InlineData(5, 47635)]
    [InlineData(6, 899442)]
    public void Position20_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k3/8/8/8/8/8/8/4K2R b K - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 80)]
    [InlineData(3, 493)]
    [InlineData(4, 8897)]
    [InlineData(5, 52710)]
    [InlineData(6, 1001523)]
    public void Position21_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k3/8/8/8/8/8/8/R3K3 b Q - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 15)]
    [InlineData(2, 66)]
    [InlineData(3, 1197)]
    [InlineData(4, 7059)]
    [InlineData(5, 133987)]
    [InlineData(6, 764643)]
    public void Position22_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k2r/8/8/8/8/8/8/4K3 b k - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 16)]
    [InlineData(2, 71)]
    [InlineData(3, 1287)]
    [InlineData(4, 7626)]
    [InlineData(5, 145232)]
    [InlineData(6, 846648)]
    public void Position23_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k3/8/8/8/8/8/8/4K3 b q - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 130)]
    [InlineData(3, 782)]
    [InlineData(4, 22180)]
    [InlineData(5, 118882)]
    [InlineData(6, 3517770)]
    public void Position24_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k3/8/8/8/8/8/8/R3K2R b KQ - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 26)]
    [InlineData(2, 112)]
    [InlineData(3, 3189)]
    [InlineData(4, 17945)]
    [InlineData(5, 532933)]
    [InlineData(6, 2788982)]
    public void Position25_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/4K3 b kq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 32)]
    [InlineData(3, 134)]
    [InlineData(4, 2073)]
    [InlineData(5, 10485)]
    [InlineData(6, 179869)]
    public void Position26_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/8/6k1/4K2R b K - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 49)]
    [InlineData(3, 243)]
    [InlineData(4, 3991)]
    [InlineData(5, 20780)]
    [InlineData(6, 367724)]
    public void Position27_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/8/1k6/R3K3 b Q - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 12)]
    [InlineData(2, 38)]
    [InlineData(3, 564)]
    [InlineData(4, 2219)]
    [InlineData(5, 37735)]
    [InlineData(6, 185867)]
    public void Position28_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k2r/6K1/8/8/8/8/8/8 b k - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 15)]
    [InlineData(2, 65)]
    [InlineData(3, 1018)]
    [InlineData(4, 4573)]
    [InlineData(5, 80619)]
    [InlineData(6, 413018)]
    public void Position29_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k3/1K6/8/8/8/8/8/8 b q - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 26)]
    [InlineData(2, 568)]
    [InlineData(3, 13744)]
    [InlineData(4, 314346)]
    [InlineData(5, 7594526)]
    [InlineData(6, 179862938)]
    public void Position30_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/R3K2R b KQkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 26)]
    [InlineData(2, 583)]
    [InlineData(3, 14252)]
    [InlineData(4, 334705)]
    [InlineData(5, 8198901)]
    [InlineData(6, 198328929)]
    public void Position31_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/1R2K2R b Kkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 25)]
    [InlineData(2, 560)]
    [InlineData(3, 13592)]
    [InlineData(4, 317324)]
    [InlineData(5, 7710115)]
    [InlineData(6, 185959088)]
    public void Position32_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/2R1K2R b Kkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 25)]
    [InlineData(2, 560)]
    [InlineData(3, 13607)]
    [InlineData(4, 320792)]
    [InlineData(5, 7848606)]
    [InlineData(6, 190755813)]
    public void Position33_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k2r/8/8/8/8/8/8/R3K1R1 b Qkq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 25)]
    [InlineData(2, 567)]
    [InlineData(3, 14095)]
    [InlineData(4, 328965)]
    [InlineData(5, 8153719)]
    [InlineData(6, 195629489)]
    public void Position34_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("1r2k2r/8/8/8/8/8/8/R3K2R b KQk - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 25)]
    [InlineData(2, 548)]
    [InlineData(3, 13502)]
    [InlineData(4, 312835)]
    [InlineData(5, 7736373)]
    [InlineData(6, 184411439)]
    public void Position35_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("2r1k2r/8/8/8/8/8/8/R3K2R b KQk - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 25)]
    [InlineData(2, 547)]
    [InlineData(3, 13579)]
    [InlineData(4, 316214)]
    [InlineData(5, 7878456)]
    [InlineData(6, 189224276)]
    public void Position36_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("r3k1r1/8/8/8/8/8/8/R3K2R b KQq - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 14)]
    [InlineData(2, 195)]
    [InlineData(3, 2760)]
    [InlineData(4, 38675)]
    [InlineData(5, 570726)]
    [InlineData(6, 8107539)]
    public void Position37_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/1n4N1/2k5/8/8/5K2/1N4n1/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 11)]
    [InlineData(2, 156)]
    [InlineData(3, 1636)]
    [InlineData(4, 20534)]
    [InlineData(5, 223507)]
    [InlineData(6, 2594412)]
    public void Position38_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/1k6/8/5N2/8/4n3/8/2K5 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 19)]
    [InlineData(2, 289)]
    [InlineData(3, 4442)]
    [InlineData(4, 73584)]
    [InlineData(5, 1198299)]
    [InlineData(6, 19870403)]
    public void Position39_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/4k3/3Nn3/3nN3/4K3/8/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 51)]
    [InlineData(3, 345)]
    [InlineData(4, 5301)]
    [InlineData(5, 38348)]
    [InlineData(6, 588695)]
    public void Position40_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("K7/8/2n5/1n6/8/8/8/k6N w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 17)]
    [InlineData(2, 54)]
    [InlineData(3, 835)]
    [InlineData(4, 5910)]
    [InlineData(5, 92250)]
    [InlineData(6, 688780)]
    public void Position41_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/2N5/1N6/8/8/8/K6n w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 15)]
    [InlineData(2, 193)]
    [InlineData(3, 2816)]
    [InlineData(4, 40039)]
    [InlineData(5, 582642)]
    [InlineData(6, 8503277)]
    public void Position42_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/1n4N1/2k5/8/8/5K2/1N4n1/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 16)]
    [InlineData(2, 180)]
    [InlineData(3, 2290)]
    [InlineData(4, 24640)]
    [InlineData(5, 288141)]
    [InlineData(6, 3147566)]
    public void Position43_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/1k6/8/5N2/8/4n3/8/2K5 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 68)]
    [InlineData(3, 1118)]
    [InlineData(4, 16199)]
    [InlineData(5, 281190)]
    [InlineData(6, 4405103)]
    public void Position44_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/3K4/3Nn3/3nN3/4k3/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 17)]
    [InlineData(2, 54)]
    [InlineData(3, 835)]
    [InlineData(4, 5910)]
    [InlineData(5, 92250)]
    [InlineData(6, 688780)]
    public void Position45_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("K7/8/2n5/1n6/8/8/8/k6N b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 51)]
    [InlineData(3, 345)]
    [InlineData(4, 5301)]
    [InlineData(5, 38348)]
    [InlineData(6, 588695)]
    public void Position46_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/2N5/1N6/8/8/8/K6n b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 17)]
    [InlineData(2, 278)]
    [InlineData(3, 4607)]
    [InlineData(4, 76778)]
    [InlineData(5, 1320507)]
    [InlineData(6, 22823890)]
    public void Position47_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("B6b/8/8/8/2K5/4k3/8/b6B w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 21)]
    [InlineData(2, 316)]
    [InlineData(3, 5744)]
    [InlineData(4, 93338)]
    [InlineData(5, 1713368)]
    [InlineData(6, 28861171)]
    public void Position48_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/1B6/7b/7k/8/2B1b3/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 21)]
    [InlineData(2, 144)]
    [InlineData(3, 3242)]
    [InlineData(4, 32955)]
    [InlineData(5, 787524)]
    [InlineData(6, 7881673)]
    public void Position49_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/B7/1B6/1B6/8/8/8/K6b w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 7)]
    [InlineData(2, 143)]
    [InlineData(3, 1416)]
    [InlineData(4, 31787)]
    [InlineData(5, 310862)]
    [InlineData(6, 7382896)]
    public void Position50_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("K7/b7/1b6/1b6/8/8/8/k6B w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 6)]
    [InlineData(2, 106)]
    [InlineData(3, 1829)]
    [InlineData(4, 31151)]
    [InlineData(5, 530585)]
    [InlineData(6, 9250746)]
    public void Position51_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("B6b/8/8/8/2K5/5k2/8/b6B b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 17)]
    [InlineData(2, 309)]
    [InlineData(3, 5133)]
    [InlineData(4, 93603)]
    [InlineData(5, 1591064)]
    [InlineData(6, 29027891)]
    public void Position52_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/1B6/7b/7k/8/2B1b3/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 7)]
    [InlineData(2, 143)]
    [InlineData(3, 1416)]
    [InlineData(4, 31787)]
    [InlineData(5, 310862)]
    [InlineData(6, 7382896)]
    public void Position53_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/B7/1B6/1B6/8/8/8/K6b b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 21)]
    [InlineData(2, 144)]
    [InlineData(3, 3242)]
    [InlineData(4, 32955)]
    [InlineData(5, 787524)]
    [InlineData(6, 7881673)]
    public void Position54_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("K7/b7/1b6/1b6/8/8/8/k6B b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 19)]
    [InlineData(2, 275)]
    [InlineData(3, 5300)]
    [InlineData(4, 104342)]
    [InlineData(5, 2161211)]
    [InlineData(6, 44956585)]
    public void Position55_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/RR6/8/8/8/8/rr6/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 36)]
    [InlineData(2, 1027)]
    [InlineData(3, 29215)]
    [InlineData(4, 771461)]
    [InlineData(5, 20506480)]
    [InlineData(6, 525169084)]
    public void Position56_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("R6r/8/8/2K5/5k2/8/8/r6R w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 19)]
    [InlineData(2, 275)]
    [InlineData(3, 5300)]
    [InlineData(4, 104342)]
    [InlineData(5, 2161211)]
    [InlineData(6, 44956585)]
    public void Position57_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/RR6/8/8/8/8/rr6/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 36)]
    [InlineData(2, 1027)]
    [InlineData(3, 29227)]
    [InlineData(4, 771368)]
    [InlineData(5, 20521342)]
    [InlineData(6, 524966748)]
    public void Position58_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("R6r/8/8/2K5/5k2/8/8/r6R b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 36)]
    [InlineData(3, 143)]
    [InlineData(4, 3637)]
    [InlineData(5, 14893)]
    [InlineData(6, 391507)]
    public void Position59_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("6kq/8/8/8/8/8/8/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 36)]
    [InlineData(3, 143)]
    [InlineData(4, 3637)]
    [InlineData(5, 14893)]
    [InlineData(6, 391507)]
    public void Position60_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("6KQ/8/8/8/8/8/8/7k b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 6)]
    [InlineData(2, 35)]
    [InlineData(3, 495)]
    [InlineData(4, 8349)]
    [InlineData(5, 166741)]
    [InlineData(6, 3370175)]
    public void Position61_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("K7/8/8/3Q4/4q3/8/8/7k w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 22)]
    [InlineData(2, 43)]
    [InlineData(3, 1015)]
    [InlineData(4, 4167)]
    [InlineData(5, 105749)]
    [InlineData(6, 419369)]
    public void Position62_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("6qk/8/8/8/8/8/8/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 36)]
    [InlineData(3, 143)]
    [InlineData(4, 3637)]
    [InlineData(5, 14893)]
    [InlineData(6, 391507)]
    public void Position63_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("6KQ/8/8/8/8/8/8/7k b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 6)]
    [InlineData(2, 35)]
    [InlineData(3, 495)]
    [InlineData(4, 8349)]
    [InlineData(5, 166741)]
    [InlineData(6, 3370175)]
    public void Position64_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("K7/8/8/3Q4/4q3/8/8/7k b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 7)]
    [InlineData(3, 43)]
    [InlineData(4, 199)]
    [InlineData(5, 1347)]
    [InlineData(6, 6249)]
    public void Position65_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/K7/P7/k7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 7)]
    [InlineData(3, 43)]
    [InlineData(4, 199)]
    [InlineData(5, 1347)]
    [InlineData(6, 6249)]
    public void Position66_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/7K/7P/7k w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 12)]
    [InlineData(4, 80)]
    [InlineData(5, 342)]
    [InlineData(6, 2343)]
    public void Position67_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("K7/p7/k7/8/8/8/8/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 12)]
    [InlineData(4, 80)]
    [InlineData(5, 342)]
    [InlineData(6, 2343)]
    public void Position68_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7K/7p/7k/8/8/8/8/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 7)]
    [InlineData(2, 35)]
    [InlineData(3, 210)]
    [InlineData(4, 1091)]
    [InlineData(5, 7028)]
    [InlineData(6, 34834)]
    public void Position69_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/2k1p3/3pP3/3P2K1/8/8/8/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 12)]
    [InlineData(4, 80)]
    [InlineData(5, 342)]
    [InlineData(6, 2343)]
    public void Position70_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/K7/P7/k7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 1)]
    [InlineData(2, 3)]
    [InlineData(3, 12)]
    [InlineData(4, 80)]
    [InlineData(5, 342)]
    [InlineData(6, 2343)]
    public void Position71_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/7K/7P/7k b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 7)]
    [InlineData(3, 43)]
    [InlineData(4, 199)]
    [InlineData(5, 1347)]
    [InlineData(6, 6249)]
    public void Position72_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("K7/p7/k7/8/8/8/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 7)]
    [InlineData(3, 43)]
    [InlineData(4, 199)]
    [InlineData(5, 1347)]
    [InlineData(6, 6249)]
    public void Position73_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7K/7p/7k/8/8/8/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 35)]
    [InlineData(3, 182)]
    [InlineData(4, 1091)]
    [InlineData(5, 5408)]
    [InlineData(6, 34822)]
    public void Position74_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/2k1p3/3pP3/3P2K1/8/8/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 8)]
    [InlineData(3, 44)]
    [InlineData(4, 282)]
    [InlineData(5, 1814)]
    [InlineData(6, 11848)]
    public void Position75_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/8/8/8/4k3/4P3/4K3 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 2)]
    [InlineData(2, 8)]
    [InlineData(3, 44)]
    [InlineData(4, 282)]
    [InlineData(5, 1814)]
    [InlineData(6, 11848)]
    public void Position76_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("4k3/4p3/4K3/8/8/8/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 9)]
    [InlineData(3, 57)]
    [InlineData(4, 360)]
    [InlineData(5, 1969)]
    [InlineData(6, 10724)]
    public void Position77_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/7k/7p/7P/7K/8/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 9)]
    [InlineData(3, 57)]
    [InlineData(4, 360)]
    [InlineData(5, 1969)]
    [InlineData(6, 10724)]
    public void Position78_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/k7/p7/P7/K7/8/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 25)]
    [InlineData(3, 180)]
    [InlineData(4, 1294)]
    [InlineData(5, 8296)]
    [InlineData(6, 53138)]
    public void Position79_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/3k4/3p4/3P4/3K4/8/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 8)]
    [InlineData(2, 61)]
    [InlineData(3, 483)]
    [InlineData(4, 3213)]
    [InlineData(5, 23599)]
    [InlineData(6, 157093)]
    public void Position80_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/3k4/3p4/8/3P4/3K4/8/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 8)]
    [InlineData(2, 61)]
    [InlineData(3, 411)]
    [InlineData(4, 3213)]
    [InlineData(5, 21637)]
    [InlineData(6, 158065)]
    public void Position81_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/3k4/3p4/8/3P4/3K4/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 15)]
    [InlineData(3, 90)]
    [InlineData(4, 534)]
    [InlineData(5, 3450)]
    [InlineData(6, 20960)]
    public void Position82_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/3p4/8/3P4/8/8/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 9)]
    [InlineData(3, 57)]
    [InlineData(4, 360)]
    [InlineData(5, 1969)]
    [InlineData(6, 10724)]
    public void Position83_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/7k/7p/7P/7K/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 9)]
    [InlineData(3, 57)]
    [InlineData(4, 360)]
    [InlineData(5, 1969)]
    [InlineData(6, 10724)]
    public void Position84_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/k7/p7/P7/K7/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 25)]
    [InlineData(3, 180)]
    [InlineData(4, 1294)]
    [InlineData(5, 8296)]
    [InlineData(6, 53138)]
    public void Position85_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/3k4/3p4/3P4/3K4/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 8)]
    [InlineData(2, 61)]
    [InlineData(3, 411)]
    [InlineData(4, 3213)]
    [InlineData(5, 21637)]
    [InlineData(6, 158065)]
    public void Position86_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/3k4/3p4/8/3P4/3K4/8/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 8)]
    [InlineData(2, 61)]
    [InlineData(3, 483)]
    [InlineData(4, 3213)]
    [InlineData(5, 23599)]
    [InlineData(6, 157093)]
    public void Position87_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/8/3k4/3p4/8/3P4/3K4/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 15)]
    [InlineData(3, 89)]
    [InlineData(4, 537)]
    [InlineData(5, 3309)]
    [InlineData(6, 21104)]
    public void Position88_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/3p4/8/3P4/8/8/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 19)]
    [InlineData(3, 117)]
    [InlineData(4, 720)]
    [InlineData(5, 4661)]
    [InlineData(6, 32191)]
    public void Position89_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/3p4/8/8/3P4/8/8/K7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 19)]
    [InlineData(3, 116)]
    [InlineData(4, 716)]
    [InlineData(5, 4786)]
    [InlineData(6, 30980)]
    public void Position90_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/8/3p4/8/8/3P4/K7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 22)]
    [InlineData(3, 139)]
    [InlineData(4, 877)]
    [InlineData(5, 6112)]
    [InlineData(6, 41874)]
    public void Position91_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/8/7p/6P1/8/8/K7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 16)]
    [InlineData(3, 101)]
    [InlineData(4, 637)]
    [InlineData(5, 4354)]
    [InlineData(6, 29679)]
    public void Position92_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/7p/8/8/6P1/8/K7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 22)]
    [InlineData(3, 139)]
    [InlineData(4, 877)]
    [InlineData(5, 6112)]
    [InlineData(6, 41874)]
    public void Position93_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/8/6p1/7P/8/8/K7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 16)]
    [InlineData(3, 101)]
    [InlineData(4, 637)]
    [InlineData(5, 4354)]
    [InlineData(6, 29679)]
    public void Position94_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/6p1/8/8/7P/8/K7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 3)]
    [InlineData(2, 15)]
    [InlineData(3, 84)]
    [InlineData(4, 573)]
    [InlineData(5, 3013)]
    [InlineData(6, 22886)]
    public void Position95_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/8/3p4/4p3/8/8/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 16)]
    [InlineData(3, 101)]
    [InlineData(4, 637)]
    [InlineData(5, 4271)]
    [InlineData(6, 28662)]
    public void Position96_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/3p4/8/8/4P3/8/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 19)]
    [InlineData(3, 117)]
    [InlineData(4, 720)]
    [InlineData(5, 5014)]
    [InlineData(6, 32167)]
    public void Position97_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/3p4/8/8/3P4/8/8/K7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 19)]
    [InlineData(3, 117)]
    [InlineData(4, 712)]
    [InlineData(5, 4658)]
    [InlineData(6, 30749)]
    public void Position98_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/8/3p4/8/8/3P4/K7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 22)]
    [InlineData(3, 139)]
    [InlineData(4, 877)]
    [InlineData(5, 6112)]
    [InlineData(6, 41874)]
    public void Position99_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/8/7p/6P1/8/8/K7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 16)]
    [InlineData(3, 101)]
    [InlineData(4, 637)]
    [InlineData(5, 4354)]
    [InlineData(6, 29679)]
    public void Position100_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/7p/8/8/6P1/8/K7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 22)]
    [InlineData(3, 139)]
    [InlineData(4, 877)]
    [InlineData(5, 6112)]
    [InlineData(6, 41874)]
    public void Position101_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/8/6p1/7P/8/8/K7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 16)]
    [InlineData(3, 101)]
    [InlineData(4, 637)]
    [InlineData(5, 4354)]
    [InlineData(6, 29679)]
    public void Position102_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/6p1/8/8/7P/8/K7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 15)]
    [InlineData(3, 102)]
    [InlineData(4, 569)]
    [InlineData(5, 4337)]
    [InlineData(6, 22579)]
    public void Position103_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/8/3p4/4p3/8/8/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 16)]
    [InlineData(3, 101)]
    [InlineData(4, 637)]
    [InlineData(5, 4271)]
    [InlineData(6, 28662)]
    public void Position104_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/8/3p4/8/8/4P3/8/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 22)]
    [InlineData(3, 139)]
    [InlineData(4, 877)]
    [InlineData(5, 6112)]
    [InlineData(6, 41874)]
    public void Position105_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/8/p7/1P6/8/8/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 16)]
    [InlineData(3, 101)]
    [InlineData(4, 637)]
    [InlineData(5, 4354)]
    [InlineData(6, 29679)]
    public void Position106_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/p7/8/8/1P6/8/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 22)]
    [InlineData(3, 139)]
    [InlineData(4, 877)]
    [InlineData(5, 6112)]
    [InlineData(6, 41874)]
    public void Position107_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/8/1p6/P7/8/8/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 16)]
    [InlineData(3, 101)]
    [InlineData(4, 637)]
    [InlineData(5, 4354)]
    [InlineData(6, 29679)]
    public void Position108_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/1p6/8/8/P7/8/7K w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 25)]
    [InlineData(3, 161)]
    [InlineData(4, 1035)]
    [InlineData(5, 7574)]
    [InlineData(6, 55338)]
    public void Position109_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/7p/8/8/8/8/6P1/K7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 25)]
    [InlineData(3, 161)]
    [InlineData(4, 1035)]
    [InlineData(5, 7574)]
    [InlineData(6, 55338)]
    public void Position110_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/6p1/8/8/8/8/7P/K7 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 7)]
    [InlineData(2, 49)]
    [InlineData(3, 378)]
    [InlineData(4, 2902)]
    [InlineData(5, 24122)]
    [InlineData(6, 199002)]
    public void Position111_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("3k4/3pp3/8/8/8/8/3PP3/3K4 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 22)]
    [InlineData(3, 139)]
    [InlineData(4, 877)]
    [InlineData(5, 6112)]
    [InlineData(6, 41874)]
    public void Position112_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/8/p7/1P6/8/8/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 16)]
    [InlineData(3, 101)]
    [InlineData(4, 637)]
    [InlineData(5, 4354)]
    [InlineData(6, 29679)]
    public void Position113_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/p7/8/8/1P6/8/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 22)]
    [InlineData(3, 139)]
    [InlineData(4, 877)]
    [InlineData(5, 6112)]
    [InlineData(6, 41874)]
    public void Position114_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/8/1p6/P7/8/8/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 4)]
    [InlineData(2, 16)]
    [InlineData(3, 101)]
    [InlineData(4, 637)]
    [InlineData(5, 4354)]
    [InlineData(6, 29679)]
    public void Position115_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("7k/8/1p6/8/8/P7/8/7K b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 25)]
    [InlineData(3, 161)]
    [InlineData(4, 1035)]
    [InlineData(5, 7574)]
    [InlineData(6, 55338)]
    public void Position116_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/7p/8/8/8/8/6P1/K7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 5)]
    [InlineData(2, 25)]
    [InlineData(3, 161)]
    [InlineData(4, 1035)]
    [InlineData(5, 7574)]
    [InlineData(6, 55338)]
    public void Position117_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("k7/6p1/8/8/8/8/7P/K7 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 7)]
    [InlineData(2, 49)]
    [InlineData(3, 378)]
    [InlineData(4, 2902)]
    [InlineData(5, 24122)]
    [InlineData(6, 199002)]
    public void Position118_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("3k4/3pp3/8/8/8/8/3PP3/3K4 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 11)]
    [InlineData(2, 97)]
    [InlineData(3, 887)]
    [InlineData(4, 8048)]
    [InlineData(5, 90606)]
    [InlineData(6, 1030499)]
    public void Position119_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/Pk6/8/8/8/8/6Kp/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 24)]
    [InlineData(2, 421)]
    [InlineData(3, 7421)]
    [InlineData(4, 124608)]
    [InlineData(5, 2193768)]
    [InlineData(6, 37665329)]
    public void Position120_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("n1n5/1Pk5/8/8/8/8/5Kp1/5N1N w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 18)]
    [InlineData(2, 270)]
    [InlineData(3, 4699)]
    [InlineData(4, 79355)]
    [InlineData(5, 1533145)]
    [InlineData(6, 28859283)]
    public void Position121_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/PPPk4/8/8/8/8/4Kppp/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 24)]
    [InlineData(2, 496)]
    [InlineData(3, 9483)]
    [InlineData(4, 182838)]
    [InlineData(5, 3605103)]
    [InlineData(6, 71179139)]
    public void Position122_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("n1n5/PPPk4/8/8/8/8/4Kppp/5N1N w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 11)]
    [InlineData(2, 97)]
    [InlineData(3, 887)]
    [InlineData(4, 8048)]
    [InlineData(5, 90606)]
    [InlineData(6, 1030499)]
    public void Position123_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/Pk6/8/8/8/8/6Kp/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 24)]
    [InlineData(2, 421)]
    [InlineData(3, 7421)]
    [InlineData(4, 124608)]
    [InlineData(5, 2193768)]
    [InlineData(6, 37665329)]
    public void Position124_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("n1n5/1Pk5/8/8/8/8/5Kp1/5N1N b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 18)]
    [InlineData(2, 270)]
    [InlineData(3, 4699)]
    [InlineData(4, 79355)]
    [InlineData(5, 1533145)]
    [InlineData(6, 28859283)]
    public void Position125_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/PPPk4/8/8/8/8/4Kppp/8 b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(1, 24)]
    [InlineData(2, 496)]
    [InlineData(3, 9483)]
    [InlineData(4, 182838)]
    [InlineData(5, 3605103)]
    [InlineData(6, 71179139)]
    public void Position126_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("n1n5/PPPk4/8/8/8/8/4Kppp/5N1N b - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(4, 43238)]
    [InlineData(5, 674624)]
    [InlineData(6, 11030083)]
    public void Position127_ShouldReturnCorrectPerftCount(int depth, long expectedNodes)
    {
        // Arrange
        var position = new Position("8/2p5/3p4/KP5r/1R3p1k/8/4P1P1/8 w - - 0 1");

        // Act
        var nodes = CountNodes(position, _executor, depth);

        // Assert
        nodes.ShouldBe(expectedNodes);
    }

    [Theory]
    [InlineData(5, 11139762)]
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
