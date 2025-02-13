﻿// See https://aka.ms/new-console-template for more information

using Zugfish.Engine;
using Zugfish.SelfPlay;

var board = new Board();
var moveGenerator = new MoveGenerator();

while (true)
{
    Console.WriteLine(board.Render());
    var bestMove = Search.FindBestMove(moveGenerator, board, 4);
    board.MakeMove(bestMove);
}
