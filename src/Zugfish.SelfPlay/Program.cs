// See https://aka.ms/new-console-template for more information

using Zugfish.Engine;
using Zugfish.SelfPlay;

// var board = new Board();
// Console.WriteLine("Initial Board:");
// Console.WriteLine(board.Render());
//
// Move move = new Move("e2e4");
// board.MakeMove(move);
// Console.WriteLine("After Move:");
// Console.WriteLine(board.Render());
//
// board.UnmakeMove();
// Console.WriteLine("After Unmaking Move:");
// Console.WriteLine(board.Render());

var board = new Board("r1bqkb1r/pppp1ppp/2n2n2/1B2p3/4P3/5N2/PPPP1PPP/RNBQK2R w KQkq - 4 4");
Console.WriteLine("Initial Board:");
Console.WriteLine(board.Render());

var move = new Move("e1g1");
board.MakeMove(move);
Console.WriteLine("After Move:");
Console.WriteLine(board.Render());

board.UnmakeMove();
Console.WriteLine("After Unmaking Move:");
Console.WriteLine(board.Render());
