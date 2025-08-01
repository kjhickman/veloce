using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Veloce.Engine;
using Veloce.Search;

namespace Veloce.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[BenchmarkCategory("Search")]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class SearchBenchmarks
{
    private VeloceEngine _veloceEngine = null!;

    [IterationSetup]
    public void IterationSetup()
    {
        _veloceEngine = new VeloceEngine(new EngineSettings { MaxDepth = 6 });
        var move = _veloceEngine.FindBestMove();
        _veloceEngine.MakeMove(move!.Value);
    }

    [Benchmark]
    public void Play10Moves()
    {
        var i = 0;
        while (i++ < 10)
        {
            var bestMove = _veloceEngine.FindBestMove();
            if (bestMove == null) throw new Exception("Game ended unexpectedly");
            _veloceEngine.MakeMove(bestMove.Value);
        }
    }
}
