using ChessLite;
using Veloce.Engine;
using Veloce.Search.Transposition;

namespace Veloce.Search;

internal sealed class LazySmpSearch
{
    private const int MinThreadCount = 1;
    private const int MaxThreadCount = 128;
    private readonly TranspositionTable _transpositions = new();
    private int _threadCount = MinThreadCount;

    public int ThreadCount => Volatile.Read(ref _threadCount);

    public int HashFull => _transpositions.HashFull;

    public static int MaximumThreadCount => MaxThreadCount;

    public void ClearHash()
    {
        _transpositions.Clear();
    }

    public void SetHashSize(int megabytes)
    {
        _transpositions.Resize(megabytes);
    }

    public void SetThreadCount(int threadCount)
    {
        Volatile.Write(ref _threadCount, Math.Clamp(threadCount, MinThreadCount, MaxThreadCount));
    }

    public SearchResult FindBestMove(
        Game game,
        SearchSettings settings,
        Action<SearchInfo>? searchInfo = null,
        CancellationToken cancellationToken = default)
    {
        _transpositions.NewSearch();

        var threadCount = ThreadCount;
        if (threadCount == 1)
        {
            var result = new Negamax(_transpositions).FindBestMove(game.Clone(), settings, WithHashFull(searchInfo), cancellationToken);
            return result with { HashFull = HashFull };
        }

        using var helperCancellation = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        var helpers = new Task<SearchResult>[threadCount - 1];
        for (var i = 0; i < helpers.Length; i++)
        {
            var helperGame = game.Clone();
            var rootMoveOffset = i + 1;
            var depthOffset = i + 1;
            helpers[i] = Task.Run(
                () => new Negamax(_transpositions, rootMoveOffset, depthOffset).FindBestMove(helperGame, settings, null, helperCancellation.Token),
                CancellationToken.None);
        }

        var mainResult = new Negamax(_transpositions).FindBestMove(game.Clone(), settings, WithHashFull(searchInfo), helperCancellation.Token);
        helperCancellation.Cancel();

        var bestResult = mainResult;
        var nodes = mainResult.Nodes;
        foreach (var helper in helpers)
        {
            try
            {
                var helperResult = helper.GetAwaiter().GetResult();
                nodes += helperResult.Nodes;
                if (helperResult.Depth > bestResult.Depth)
                {
                    bestResult = helperResult;
                }
            }
            catch (OperationCanceledException)
            {
            }
        }

        return bestResult with { Nodes = nodes, HashFull = HashFull };
    }

    private Action<SearchInfo>? WithHashFull(Action<SearchInfo>? searchInfo)
    {
        return searchInfo is null ? null : info => searchInfo(info with { HashFull = HashFull });
    }
}
