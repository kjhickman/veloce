using Zugfish.Engine;

namespace Zugfish.Uci.Lib.Extensions;

public static class MoveExecutorExtensions
{
    public static void MakeMove(this MoveExecutor executor, Position position, ReadOnlySpan<char> uciMove)
    {
        var move = Helpers.MoveFromUci(position, uciMove);
        executor.MakeMove(position, move);
    }
}
