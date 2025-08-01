using System.Diagnostics;
using Veloce.Engine;
using Veloce.Search;

var settings = new EngineSettings
{
    MaxDepth = 6,
    TranspositionTableSizeMb = 128,
    Threads = 1
};
var engine = new VeloceEngine(new ConsoleEngineLogger(), settings);
var i = 0;

var sw = Stopwatch.StartNew();
while (i++ < 40)
{
    var bestMove = engine.FindBestMove();
    if (bestMove == null)
    {
        Console.WriteLine("Game over");
        break;
    }

    engine.MakeMove(bestMove.Value);
}

Console.WriteLine($"Elapsed: {sw.Elapsed}");
