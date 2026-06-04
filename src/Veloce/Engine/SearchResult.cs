using ChessLite.Movement;

namespace Veloce.Engine;

public sealed record SearchResult(Move? BestMove, int Score, int Depth, long Nodes, TimeSpan Elapsed, int HashFull = 0);
