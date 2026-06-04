namespace Veloce.Search;

public sealed partial class Negamax
{
    private const int MateScore = 100_000;
    private const int MateThreshold = MateScore - 1_000;

    private const int MaxSearchPly = 128;
    private const int MaxQuiescencePly = 8;

    private const int InitialAspirationWindow = 25;
    private const int MaxAspirationWindow = MateScore * 2;

    private const int NullMoveMinDepth = 3;
    private const int NullMoveReduction = 2;

    private const int LateMoveReductionMinDepth = 3;
    private const int LateMoveReductionMinSearchedMoves = 4;
    private const int LateMoveReduction = 1;

    private const int TranspositionMoveScore = 1_000_000;
    private const int CaptureOrderingBaseScore = 100_000;
    private const int CaptureOrderingVictimMultiplier = 16;
    private const int PrimaryKillerMoveScore = 90_000;
    private const int SecondaryKillerMoveScore = 80_000;
    private const int MaxHistoryMoveScore = 70_000;
    private const int KingOrderingValue = 20_000;

    private const ulong HalfmoveHashMultiplier = 0x9E37_79B9_7F4A_7C15UL;
}
