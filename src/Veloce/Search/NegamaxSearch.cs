using System.Diagnostics;
using ChessLite;
using ChessLite.Movement;
using ChessLite.Primitives;
using Veloce.Engine;
using Veloce.Evaluation;
using Veloce.Search.Transposition;

namespace Veloce.Search;

public sealed partial class NegamaxSearch
{
    private const int MateScore = 100_000;
    private const int MateThreshold = MateScore - 1_000;
    private const int MaxQuiescencePly = 8;
    private const int MaxSearchPly = 128;
    private const int InitialAspirationWindow = 25;
    private const int MaxAspirationWindow = MateScore * 2;
    private const int NullMoveMinDepth = 3;
    private const int NullMoveReduction = 2;
    private const int LateMoveReductionMinDepth = 3;
    private const int LateMoveReductionMoveNumber = 4;
    private const int LateMoveReduction = 1;
    private const int TableMoveScore = 1_000_000;
    private const int CaptureMoveScore = 100_000;
    private const int PrimaryKillerScore = 90_000;
    private const int SecondaryKillerScore = 80_000;
    private const int MaxHistoryScore = 70_000;
    private const ulong HalfmoveHashMultiplier = 0x9E37_79B9_7F4A_7C15UL;
    private readonly TranspositionTable _transpositions;
    private readonly int _rootMoveOffset;
    private readonly int _depthOffset;
    private readonly CompactMove[] _primaryKillers = new CompactMove[MaxSearchPly];
    private readonly CompactMove[] _secondaryKillers = new CompactMove[MaxSearchPly];
    private readonly int[,,] _history = new int[2, 64, 64];
    private long _nodes;
    private long _nodeLimit = long.MaxValue;

    internal NegamaxSearch(TranspositionTable transpositions, int rootMoveOffset = 0, int depthOffset = 0)
    {
        _transpositions = transpositions;
        _rootMoveOffset = rootMoveOffset;
        _depthOffset = depthOffset;
    }

    public SearchResult FindBestMove(
        Game game,
        SearchSettings settings,
        Action<SearchInfo>? searchInfo = null,
        CancellationToken cancellationToken = default)
    {
        var now = Stopwatch.GetTimestamp();
        _nodes = 0;
        _nodeLimit = settings.NodeLimit ?? long.MaxValue;
        Array.Clear(_primaryKillers);
        Array.Clear(_secondaryKillers);
        Array.Clear(_history);

        using var timeLimit = settings.MoveTime.HasValue ? new CancellationTokenSource(settings.MoveTime.Value) : null;
        using var linkedCancellation = timeLimit is null
            ? null
            : CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeLimit.Token);
        var effectiveCancellation = linkedCancellation?.Token ?? cancellationToken;

        if (game.GetDrawState().IsDraw())
        {
            return new SearchResult(null, 0, 0, _nodes, Stopwatch.GetElapsedTime(now));
        }

        Span<Move> moves = stackalloc Move[218];
        var moveCount = game.WriteLegalMoves(moves);
        if (moveCount == 0)
        {
            return new SearchResult(null, EvaluateTerminal(game, 0), 0, _nodes, Stopwatch.GetElapsedTime(now));
        }

        var bestMove = moves[0];
        var bestScore = MaterialEvaluator.Evaluate(game.Position);
        var completedDepth = 0;
        var rootKey = GetTranspositionKey(game);
        var diversifiedRootMove = _rootMoveOffset == 0 ? Move.NullMove : moves[_rootMoveOffset % moveCount];

        var startDepth = Math.Min(settings.Depth, 1 + _depthOffset);
        for (var depth = startDepth; depth <= settings.Depth; depth++)
        {
            try
            {
                effectiveCancellation.ThrowIfCancellationRequested();

                var rootTableMove = _transpositions.TryGet(rootKey, out var rootEntry)
                    ? rootEntry.Move.FindMatchingMove(moves, moveCount)
                    : Move.NullMove;
                var (depthBestMove, depthBestScore) = SearchRootWithAspiration(
                    game,
                    moves,
                    moveCount,
                    depth,
                    bestMove,
                    bestScore,
                    rootTableMove,
                    diversifiedRootMove,
                    effectiveCancellation);

                bestMove = depthBestMove;
                bestScore = depthBestScore;
                completedDepth = depth;
                _transpositions.Store(rootKey, ScoreToTable(bestScore, 0), new CompactMove(bestMove), depth, TranspositionBound.Exact);
                searchInfo?.Invoke(new SearchInfo(bestMove, bestScore, completedDepth, _nodes, Stopwatch.GetElapsedTime(now)));
            }
            catch (OperationCanceledException) when (effectiveCancellation.IsCancellationRequested)
            {
                break;
            }
            catch (NodeLimitReachedException)
            {
                break;
            }
        }

        return new SearchResult(bestMove, bestScore, completedDepth, _nodes, Stopwatch.GetElapsedTime(now));
    }

    private static ulong GetTranspositionKey(Game game)
    {
        var key = game.Position.ZobristHash ^ ((ulong)game.Position.HalfmoveClock * HalfmoveHashMultiplier);
        return key == 0 ? 1 : key;
    }

    private static int ScoreToTable(int score, int ply)
    {
        if (score > MateThreshold) return score + ply;
        if (score < -MateThreshold) return score - ply;
        return score;
    }

    private static int ScoreFromTable(int score, int ply)
    {
        if (score > MateThreshold) return score - ply;
        if (score < -MateThreshold) return score + ply;
        return score;
    }

}
