using BenchmarkDotNet.Attributes;

namespace Veloce.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[BenchmarkCategory("Search")]
public class SearchBenchmarks
{
    private Search _search = null!;
    private Game _game = null!;

    [IterationSetup]
    public void IterationSetup()
    {
        _search = new Search(new NullEngineLogger());
        _game = new Game();
    }

    [Benchmark]
    public ulong Play10Moves_StartingPosition()
    {
        var i = 0;
        while (i++ < 10)
        {
            var bestMove = _search.FindBestMove(_game, 6).BestMove;
            if (bestMove == null) throw new Exception("Game ended unexpectedly");
            _game.MakeMove(bestMove.Value);
        }

        return _game.Position.ZobristHash;
    }
}
