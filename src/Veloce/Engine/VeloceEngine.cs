using ChessLite;
using ChessLite.Movement;
using ChessLite.State;
using Veloce.Search;

namespace Veloce.Engine;

public class VeloceEngine
{
    private readonly NegamaxSearch _search = new();
    private Game _game = new();

    public SearchResult FindBestMove(SearchSettings? settings = null, CancellationToken cancellationToken = default)
    {
        return _search.FindBestMove(_game, settings ?? SearchSettings.Default, cancellationToken);
    }

    public bool WhiteToMove => _game.Position.WhiteToMove;

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
