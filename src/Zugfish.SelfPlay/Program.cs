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

var board = new Board("3k4/6P1/8/8/8/8/8/3K4 w - - 0 1");
Console.WriteLine("Initial Board:");
Console.WriteLine(board.Render());

board.MakeMove("g7g8q");
Console.WriteLine("After Move:");
Console.WriteLine(board.Render());

board.UndoMove();
Console.WriteLine("After Unmaking Move:");
Console.WriteLine(board.Render());
