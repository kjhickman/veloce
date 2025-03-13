namespace Veloce.Uci.Lib.Extensions;

public static class GameExtensions
{
    public static void MakeMove(this Game game, ReadOnlySpan<char> uciMove)
    {
        var move = Helpers.MoveFromUci(game.Position, uciMove);
        game.MakeMove(move);
    }
}
