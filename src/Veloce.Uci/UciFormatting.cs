using ChessLite.Movement;
using ChessLite.Primitives;
using Veloce.Engine;

namespace Veloce.Uci;

public static class UciFormatting
{
    public static string FormatMove(Move move)
    {
        if (move == Move.NullMove)
        {
            return "0000";
        }

        var promotion = move.PromotedPieceType switch
        {
            PromotedPieceType.None => string.Empty,
            PromotedPieceType.Knight => "n",
            PromotedPieceType.Bishop => "b",
            PromotedPieceType.Rook => "r",
            PromotedPieceType.Queen => "q",
            _ => throw new ArgumentOutOfRangeException(nameof(move), "Unknown promotion piece."),
        };

        return $"{move.From}{move.To}{promotion}";
    }

    public static string FormatSearchInfo(SearchInfo info)
    {
        return FormatInfoLine(info.Depth, info.Score, info.Nodes, info.Elapsed, info.HashFull, info.BestMove);
    }

    public static string FormatSearchResult(SearchResult result)
    {
        return FormatInfoLine(result.Depth, result.Score, result.Nodes, result.Elapsed, result.HashFull, result.BestMove);
    }

    private static string FormatInfoLine(int depth, int score, long nodes, TimeSpan elapsed, int hashFull, Move? bestMove)
    {
        var elapsedMilliseconds = (long)elapsed.TotalMilliseconds;
        var nodesPerSecond = elapsedMilliseconds > 0 ? nodes * 1000 / elapsedMilliseconds : nodes;
        var line = $"info depth {depth} multipv 1 score cp {score} nodes {nodes} nps {nodesPerSecond} time {elapsedMilliseconds} hashfull {hashFull}";
        return bestMove.HasValue ? $"{line} pv {FormatMove(bestMove.Value)}" : line;
    }
}
