using System.Diagnostics;
using Veloce;

var settings = new EngineSettings
{
    Depth = 8,
    TranspositionTableSizeMb = 16,
    Threads = 1
};
var engine = new Engine(new ConsoleEngineLogger(), settings);
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
