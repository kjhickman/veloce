using BenchmarkDotNet.Attributes;
using Zugfish.Engine;

namespace Zugfish.Benchmarks.Benchmarks;

[MemoryDiagnoser]
[BenchmarkCategory("StaticAnalysis")]
public class StaticAnalysisBenchmarks
{
    private readonly Position _position = new("r1bqk2r/1pp2ppp/2n2n2/p2pN3/QbB5/2N5/PP3PPP/R1B2RK1 w kq - 0 11");

    [Benchmark]
    public int EvaluatePosition()
    {
        return _position.Evaluate();
    }

    [Benchmark]
    public ulong ComputeZobristHash()
    {
        return _position.ComputeZobristHash();
    }
}
