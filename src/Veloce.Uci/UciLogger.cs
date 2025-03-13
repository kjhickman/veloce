using System.Text;
using Veloce;
using Veloce.Models;

namespace Veloce.Uci;

public class UciEngineLogger : IEngineLogger
{
    // TODO: use source generated logger to avoid allocations??
    public void LogSearchInfo(SearchInfo info)
    {
        var sb = new StringBuilder("info");
        sb.Append($" depth {info.Depth}");

        // Add score information (either centipawns or mate)
        if (info.IsMateScore)
        {
            var mateIn = info.Score > 0
                ? (10000 - info.Score + 1) / 2
                : (-10000 - info.Score + 1) / 2;
            sb.Append($" score mate {mateIn}");
        }
        else
        {
            sb.Append($" score cp {info.Score}");
        }

        sb.Append($" nodes {info.NodesSearched}");
        sb.Append($" time {(int)info.TimeElapsed.TotalMilliseconds}");
        sb.Append($" nps {info.NodesPerSecond}");
        sb.Append($" hashfull {info.HashFull}");

        Console.WriteLine(sb.ToString());
    }

    public void LogBestMove(Move? bestMove, Move? ponderMove = null)
    {
        var sb = new StringBuilder("bestmove");

        if (bestMove.HasValue && bestMove.Value != Move.NullMove)
        {
            sb.Append($" {bestMove.Value}");

            if (ponderMove.HasValue && ponderMove.Value != Move.NullMove)
            {
                sb.Append($" ponder {ponderMove.Value}");
            }
        }
        else
        {
            // No legal moves - output a null move as required by UCI
            sb.Append(" 0000");
        }

        Console.WriteLine(sb.ToString());
    }
}
