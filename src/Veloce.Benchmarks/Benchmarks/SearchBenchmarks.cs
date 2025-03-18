using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;

namespace Veloce.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[BenchmarkCategory("Search")]
[SimpleJob(RuntimeMoniker.Net80)]
[SimpleJob(RuntimeMoniker.Net90)]
[SimpleJob(RuntimeMoniker.Net10_0)]
public class SearchBenchmarks
{
    private Engine _engine = null!;

    [IterationSetup]
    public void IterationSetup()
    {
        _engine = new Engine(new NullEngineLogger(), new EngineSettings { MaxDepth = 6 });
        var move = _engine.FindBestMove();
        _engine.MakeMove(move!.Value);
    }

    [Benchmark]
    public void Play10Moves()
    {
        var i = 0;
        while (i++ < 10)
        {
            var bestMove = _engine.FindBestMove();
            if (bestMove == null) throw new Exception("Game ended unexpectedly");
            _engine.MakeMove(bestMove.Value);
        }
    }
}
