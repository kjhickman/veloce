using System.Diagnostics;
using Veloce.Engine;
using Veloce.Search;
using Veloce.Search.Logging;

var settings = new EngineSettings
{
    MaxDepth = 99,
    TranspositionTableSizeMb = 128,
    Threads = Environment.ProcessorCount,
};
var engine = new VeloceEngine(new NullEngineLogger(), settings);
var i = 0;

var sw = Stopwatch.StartNew();
while (i++ < 40)
{
    var searchResult = engine.FindBestMove(5000);
    Console.WriteLine($"Search completed in {searchResult.TimeElapsed.TotalMilliseconds}ms, nodes: {searchResult.NodesSearched}, depth: {searchResult.Depth} best move: {searchResult.BestMove}");
    if (searchResult.BestMove == null)
    {
        Console.WriteLine("Game over");
        break;
    }

    engine.MakeMove(searchResult.BestMove.Value);
}

Console.WriteLine($"Elapsed: {sw.Elapsed}");
