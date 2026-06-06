using ChessLite.Movement;
using ChessLite.Primitives;
using Veloce.Engine;

namespace Veloce.Uci;

public static class UciFormatting
{
    private const int MateScore = 100_000;
    private const int MateThreshold = MateScore - 1_000;

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
        return FormatInfoLine(info.Depth, info.SelectiveDepth, info.Score, info.Nodes, info.Elapsed, info.HashFull, info.BestMove, info.PrincipalVariation);
    }

    public static string FormatSearchResult(SearchResult result)
    {
        return FormatInfoLine(result.Depth, result.SelectiveDepth, result.Score, result.Nodes, result.Elapsed, result.HashFull, result.BestMove, result.PrincipalVariation);
    }

    private static string FormatInfoLine(int depth, int selectiveDepth, int score, long nodes, TimeSpan elapsed, int hashFull, Move? bestMove, Move[]? principalVariation)
    {
        var elapsedMilliseconds = (long)elapsed.TotalMilliseconds;
        var nodesPerSecond = elapsedMilliseconds > 0 ? nodes * 1000 / elapsedMilliseconds : nodes;
        var line = $"info depth {depth} seldepth {selectiveDepth} multipv 1 score {FormatScore(score)} nodes {nodes} nps {nodesPerSecond} time {elapsedMilliseconds} hashfull {hashFull}";

        if (principalVariation is { Length: > 0 })
        {
            return $"{line} pv {string.Join(' ', principalVariation.Select(FormatMove))}";
        }

        return bestMove.HasValue ? $"{line} pv {FormatMove(bestMove.Value)}" : line;
    }

    private static string FormatScore(int score)
    {
        if (score > MateThreshold)
        {
            return $"mate {(MateScore - score + 1) / 2}";
        }

        if (score < -MateThreshold)
        {
            return $"mate {-((MateScore + score + 1) / 2)}";
        }

        return $"cp {score}";
    }
}
