using Veloce.Engine;

namespace Veloce.Uci;

internal static class GoCommand
{
    public static ValueTask HandleAsync(VeloceEngine engine, UciSearchSession searchSession, string[] commandParts)
    {
        var settings = ParseSearchSettings(engine, commandParts);
        return searchSession.StartAsync((cancellationToken, searchInfo) => engine.FindBestMove(settings, searchInfo, cancellationToken));
    }

    private static SearchSettings ParseSearchSettings(VeloceEngine engine, string[] commandParts)
    {
        var depth = SearchSettings.Default.Depth;
        var hasDepth = false;
        int? whiteTime = null;
        int? blackTime = null;
        var whiteIncrement = 0;
        var blackIncrement = 0;
        int? moveTime = null;
        var movesToGo = 30;
        var infinite = false;

        for (var i = 1; i < commandParts.Length - 1; i++)
        {
            if (commandParts[i].Equals("depth", StringComparison.OrdinalIgnoreCase)
                && int.TryParse(commandParts[i + 1], out var parsedDepth))
            {
                depth = Math.Max(1, parsedDepth);
                hasDepth = true;
            }
            else if (commandParts[i].Equals("movetime", StringComparison.OrdinalIgnoreCase)
                && int.TryParse(commandParts[i + 1], out var parsedMoveTime))
            {
                moveTime = Math.Max(1, parsedMoveTime);
            }
            else if (commandParts[i].Equals("wtime", StringComparison.OrdinalIgnoreCase)
                && int.TryParse(commandParts[i + 1], out var parsedWhiteTime))
            {
                whiteTime = Math.Max(0, parsedWhiteTime);
            }
            else if (commandParts[i].Equals("btime", StringComparison.OrdinalIgnoreCase)
                && int.TryParse(commandParts[i + 1], out var parsedBlackTime))
            {
                blackTime = Math.Max(0, parsedBlackTime);
            }
            else if (commandParts[i].Equals("winc", StringComparison.OrdinalIgnoreCase)
                && int.TryParse(commandParts[i + 1], out var parsedWhiteIncrement))
            {
                whiteIncrement = Math.Max(0, parsedWhiteIncrement);
            }
            else if (commandParts[i].Equals("binc", StringComparison.OrdinalIgnoreCase)
                && int.TryParse(commandParts[i + 1], out var parsedBlackIncrement))
            {
                blackIncrement = Math.Max(0, parsedBlackIncrement);
            }
            else if (commandParts[i].Equals("movestogo", StringComparison.OrdinalIgnoreCase)
                && int.TryParse(commandParts[i + 1], out var parsedMovesToGo))
            {
                movesToGo = Math.Max(1, parsedMovesToGo);
            }
        }

        for (var i = 1; i < commandParts.Length; i++)
        {
            if (commandParts[i].Equals("infinite", StringComparison.OrdinalIgnoreCase))
            {
                infinite = true;
                break;
            }
        }

        if (infinite)
        {
            return new SearchSettings(int.MaxValue, Infinite: true);
        }

        if (moveTime.HasValue)
        {
            return new SearchSettings(hasDepth ? depth : int.MaxValue, TimeSpan.FromMilliseconds(moveTime.Value));
        }

        var remaining = engine.WhiteToMove ? whiteTime : blackTime;
        if (remaining.HasValue)
        {
            var increment = engine.WhiteToMove ? whiteIncrement : blackIncrement;
            var budget = CalculateMoveTime(remaining.Value, increment, movesToGo);
            return new SearchSettings(hasDepth ? depth : int.MaxValue, budget);
        }

        return new SearchSettings(depth);
    }

    private static TimeSpan CalculateMoveTime(int remainingMilliseconds, int incrementMilliseconds, int movesToGo)
    {
        var reserve = Math.Min(1000, remainingMilliseconds / 20);
        var usable = Math.Max(1, remainingMilliseconds - reserve);
        var budget = usable / Math.Max(1, movesToGo) + (incrementMilliseconds * 3 / 4);
        return TimeSpan.FromMilliseconds(Math.Max(1, Math.Min(budget, usable)));
    }
}
