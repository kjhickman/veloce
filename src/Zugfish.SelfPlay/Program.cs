// See https://aka.ms/new-console-template for more information

using Zugfish.Engine;
using Zugfish.SelfPlay;

// var board = new Board();
// Console.WriteLine("Initial Board:");
// Console.WriteLine(board.Render());
//
// board.MakeMove("e2e4");
// Console.WriteLine("After Move:");
// Console.WriteLine(board.Render());
//
// board.UnmakeMove();
// Console.WriteLine("After Unmaking Move:");
// Console.WriteLine(board.Render());

var board = new Board("7k/8/8/8/Pp6/8/8/1K6 b - a3 0 1");
Console.WriteLine("Initial Board:");
Console.WriteLine(board.Render());

board.MakeMove("b4a3");
Console.WriteLine("After Move:");
Console.WriteLine(board.Render());

board.UnmakeMove();
Console.WriteLine("After Unmaking Move:");
Console.WriteLine(board.Render());
