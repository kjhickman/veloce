// See https://aka.ms/new-console-template for more information

using Zugfish.Engine;
using Zugfish.SelfPlay;

var position = new Position();
var moveGenerator = new MoveGenerator();

while (true)
{
    Console.WriteLine(position.Render());
    var bestMove = Search.FindBestMove(moveGenerator, position, 4);
    if (bestMove == null)
    {
        Console.WriteLine("Game over");
        break;
    }

    position.MakeMove(bestMove.Value);
}
