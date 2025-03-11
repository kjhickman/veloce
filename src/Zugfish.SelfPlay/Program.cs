using System.Diagnostics;
using Zugfish.Engine;
using Zugfish.SelfPlay;

var settings = EngineSettings.Default;
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
