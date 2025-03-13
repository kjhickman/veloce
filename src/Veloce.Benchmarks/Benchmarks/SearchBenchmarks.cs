using BenchmarkDotNet.Attributes;
using Veloce;

namespace Veloce.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[BenchmarkCategory("Search")]
public class SearchBenchmarks
{
    private Position _startingPosition = null!;
    private Search _search = null!;
    private MoveExecutor _executor = null!;

    // [Params(3, 4)]
    // public int Depth { get; set; }

    [IterationSetup]
    public void IterationSetup()
    {
        _startingPosition = new Position();
        _executor = new MoveExecutor();
        _search = new Search(new NullEngineLogger(), _executor, 24);
    }

    [IterationCleanup]
    public void IterationCleanup()
    {
        _executor.ClearMoveHistory();
    }

    [Benchmark]
    public ulong Play10Moves_StartingPosition()
    {
        var i = 0;
        while (i++ < 10)
        {
            var bestMove = _search.FindBestMove(_startingPosition, 6).BestMove;
            if (bestMove == null) throw new Exception("Game ended unexpectedly");
            _executor.MakeMove(_startingPosition, bestMove.Value);
        }

        return _startingPosition.ZobristHash;
    }
}
