using System.Diagnostics;
using Zugfish.Engine;
using Zugfish.SelfPlay;

var position = new Position();
var search = new Search();
var executor = new MoveExecutor();
var i = 0;

var sw = Stopwatch.StartNew();
while (i++ < 40)
{
    Console.WriteLine(position.Render());
    var bestMove = search.FindBestMove(position, 5);
    if (bestMove.BestMove == null)
    {
        Console.WriteLine("Game over");
        break;
    }

    executor.MakeMove(position, bestMove.BestMove!.Value);
}

Console.WriteLine($"Elapsed: {sw.Elapsed}");
