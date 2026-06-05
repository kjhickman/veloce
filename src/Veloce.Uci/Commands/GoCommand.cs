using Veloce.Engine;

namespace Veloce.Uci.Commands;

internal static class GoCommand
{
    public static ValueTask HandleAsync(VeloceEngine engine, SearchSession session, UciWriter output, string[] commandParts)
    {
        if (TryHandlePerft(engine, output, commandParts))
        {
            return ValueTask.CompletedTask;
        }

        var settings = ParseSearchSettings(engine, commandParts);
        return session.StartAsync((cancellationToken, searchInfo) => engine.FindBestMove(settings, searchInfo, cancellationToken));
    }

    private static bool TryHandlePerft(VeloceEngine engine, UciWriter output, string[] commandParts)
    {
        for (var i = 1; i < commandParts.Length - 1; i++)
        {
            if (!IsToken(commandParts[i], "perft") || !int.TryParse(commandParts[i + 1], out var depth))
            {
                continue;
            }

            var nodes = engine.Perft(Math.Max(0, depth), (move, moveNodes) =>
            {
                output.WriteLine($"{move}: {moveNodes}");
            });

            output.WriteLine(string.Empty);
            output.WriteLine($"Nodes searched: {nodes}");
            return true;
        }

        return false;
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
        var infinite = commandParts.Any(part => IsToken(part, "infinite"));
        var ponder = commandParts.Any(part => IsToken(part, "ponder"));
        long? nodeLimit = null;

        for (var i = 1; i < commandParts.Length - 1; i++)
        {
            var parameter = commandParts[i];
            var value = commandParts[i + 1];

            if (IsToken(parameter, "depth") && int.TryParse(value, out var parsedDepth))
            {
                depth = Math.Max(1, parsedDepth);
                hasDepth = true;
            }
            else if (IsToken(parameter, "movetime") && int.TryParse(value, out var parsedMoveTime))
            {
                moveTime = Math.Max(1, parsedMoveTime);
            }
            else if (IsToken(parameter, "wtime") && int.TryParse(value, out var parsedWhiteTime))
            {
                whiteTime = Math.Max(0, parsedWhiteTime);
            }
            else if (IsToken(parameter, "btime") && int.TryParse(value, out var parsedBlackTime))
            {
                blackTime = Math.Max(0, parsedBlackTime);
            }
            else if (IsToken(parameter, "winc") && int.TryParse(value, out var parsedWhiteIncrement))
            {
                whiteIncrement = Math.Max(0, parsedWhiteIncrement);
            }
            else if (IsToken(parameter, "binc") && int.TryParse(value, out var parsedBlackIncrement))
            {
                blackIncrement = Math.Max(0, parsedBlackIncrement);
            }
            else if (IsToken(parameter, "movestogo") && int.TryParse(value, out var parsedMovesToGo))
            {
                movesToGo = Math.Max(1, parsedMovesToGo);
            }
            else if (IsToken(parameter, "nodes") && long.TryParse(value, out var parsedNodeLimit))
            {
                nodeLimit = Math.Max(1, parsedNodeLimit);
            }
        }

        if (infinite)
        {
            return SearchSettings.InfiniteSearch(nodeLimit, ponder);
        }

        if (moveTime.HasValue)
        {
            return SearchSettings.Timed(TimeSpan.FromMilliseconds(moveTime.Value), hasDepth ? depth : null, nodeLimit, ponder);
        }

        var remaining = engine.WhiteToMove ? whiteTime : blackTime;
        if (remaining.HasValue)
        {
            var increment = engine.WhiteToMove ? whiteIncrement : blackIncrement;
            var budget = CalculateMoveTime(remaining.Value, increment, movesToGo);
            return SearchSettings.Timed(budget, hasDepth ? depth : null, nodeLimit, ponder);
        }

        return SearchSettings.DepthLimited(depth, nodeLimit, ponder);
    }

    private static TimeSpan CalculateMoveTime(int remainingMilliseconds, int incrementMilliseconds, int movesToGo)
    {
        var reserve = Math.Min(1000, remainingMilliseconds / 20);
        var usable = Math.Max(1, remainingMilliseconds - reserve);
        var baseTime = usable / Math.Max(1, movesToGo);
        var timeWithIncrement = baseTime + (incrementMilliseconds * 3 / 4);
        var budget = Math.Clamp(timeWithIncrement, 1, usable);
        return TimeSpan.FromMilliseconds(budget);
    }

    private static bool IsToken(string value, string expected)
    {
        return value.Equals(expected, StringComparison.OrdinalIgnoreCase);
    }
}
