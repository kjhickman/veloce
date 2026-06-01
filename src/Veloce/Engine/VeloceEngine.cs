using ChessLite;
using ChessLite.Movement;
using ChessLite.State;

namespace Veloce.Engine;

public class VeloceEngine(Random? random = null)
{
    private readonly Random _random = random ?? Random.Shared;
    private Game _game = new();

    public Move? FindBestMove()
    {
        Span<Move> moves = stackalloc Move[218];
        var moveCount = _game.WriteLegalMoves(moves);
        if (moveCount == 0) return null;

        return moves[_random.Next(moveCount)];
    }

    public void MakeMove(Move move)
    {
        _game.MakeMove(move);
    }

    public void MakeUciMove(string uciMove)
    {
        _game.MakeUciMove(uciMove);
    }

    public void NewGame()
    {
        _game = new Game();
    }

    public void SetPosition(Position position)
    {
        _game.ResetPosition(position);
    }

    public int WriteLegalMoves(Span<Move> moves)
    {
        return _game.WriteLegalMoves(moves);
    }
}
