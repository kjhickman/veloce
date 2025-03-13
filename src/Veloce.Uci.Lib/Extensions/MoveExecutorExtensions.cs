namespace Veloce.Uci.Lib.Extensions;

public static class MoveExecutorExtensions
{
    public static void MakeMove(this MoveExecutor moveExecutor, Position position, string uciMove)
    {
        var move = Helpers.MoveFromUci(position, uciMove);
        moveExecutor.MakeMove(position, move);
    }
}
