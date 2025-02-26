using BenchmarkDotNet.Attributes;
using Zugfish.Engine;

namespace Zugfish.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[BenchmarkCategory("Search")]
public class SearchBenchmarks
{
    private Position _startingPosition = null!;
    private Search _search = null!;

    // [Params(3, 4)]
    // public int Depth { get; set; }

    [IterationSetup]
    public void IterationSetup()
    {
        _search = new Search();
        _startingPosition = new Position();
    }

    [Benchmark]
    public ulong Play10Moves_StartingPosition()
    {
        var i = 0;
        while (i++ < 10)
        {
            var bestMove = _search.FindBestMove(_startingPosition, 4);
            if (bestMove == null) throw new Exception("Game ended unexpectedly");
            _startingPosition.MakeMove(bestMove.Value);
        }

        return _startingPosition.ZobristHash;
    }
}
