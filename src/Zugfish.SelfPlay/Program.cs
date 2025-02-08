// See https://aka.ms/new-console-template for more information

using Zugfish.Engine;
using Zugfish.SelfPlay;

var board = new Board();
Console.WriteLine("Initial Board:");
Console.WriteLine(board.Render());

Move move = new Move("e2e4");
board.MakeMove(move);
Console.WriteLine("After Move:");
Console.WriteLine(board.Render());

board.UnmakeMove();
Console.WriteLine("After Unmaking Move:");
Console.WriteLine(board.Render());
