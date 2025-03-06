using Zugfish.Engine;
using Zugfish.Perft;
using Zugfish.Uci.Lib;

var depth = int.Parse(args[0]);
var fen = args[1];
string? movesList = null;
if (args.Length == 3)
{
    movesList = args[2];
}

var position = new Position(fen);
var executor = new MoveExecutor();
if (movesList is not null)
{
    var moves = movesList.Split(' ');
    foreach (var move in moves)
    {
        executor.MakeMove(position, move);
    }
}

var division = Perft.DividePerft(position, executor, depth);

foreach (var move in division)
{
    Console.WriteLine($"{move.Key} {move.Value}");
}

Console.WriteLine($"\n{division.Sum(x => x.Value)}");
