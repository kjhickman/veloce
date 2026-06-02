using ChessLite;
using ChessLite.Movement;
using ChessLite.State;
using Veloce.Search;

namespace Veloce.Engine;

public class VeloceEngine
{
    private readonly LazySmpSearch _search = new();
    private Game _game = new();

    public SearchResult FindBestMove(
        SearchSettings? settings = null,
        Action<SearchInfo>? searchInfo = null,
        CancellationToken cancellationToken = default)
    {
        return _search.FindBestMove(_game, settings ?? SearchSettings.Default, searchInfo, cancellationToken);
    }

    public void SetHashSize(int megabytes)
    {
        _search.SetHashSize(megabytes);
    }

    public void SetThreadCount(int threadCount)
    {
        _search.SetThreadCount(threadCount);
    }

    public static int MaximumThreadCount => LazySmpSearch.MaximumThreadCount;

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
