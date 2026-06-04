using ChessLite.Movement;

namespace Veloce.Engine;

public readonly record struct SearchInfo(Move BestMove, int Score, int Depth, long Nodes, TimeSpan Elapsed, int HashFull = 0);
