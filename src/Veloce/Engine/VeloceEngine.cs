using ChessLite;
using ChessLite.Movement;
using ChessLite.State;
using Veloce.Search;
using Veloce.Search.Interfaces;

namespace Veloce.Engine;

public class VeloceEngine
{
    private MoveFinder _moveFinder;
    private readonly EngineSettings _engineSettings;
    private readonly IEngineLogger? _engineLogger;
    private Game _game;

    public VeloceEngine(EngineSettings? engineSettings = null, IEngineLogger? engineLogger = null)
    {
        _engineSettings = engineSettings ?? EngineSettings.Default;
        _engineLogger = engineLogger;
        _moveFinder = new MoveFinder(_engineSettings, _engineLogger);
        _game = new Game();
    }

    public Move? FindBestMove()
    {
        var bestMove = _moveFinder.FindBestMove(_game).BestMove;
        _engineLogger?.LogBestMove(bestMove);
        return bestMove;
    }

    public SearchResult FindBestMove(TimeControl timeControl)
    {
        var timeToSearchMs = TimeManagement.CalculateMoveTime(timeControl);
        var searchResult = _moveFinder.FindBestMove(_game, timeToSearchMs);
        _engineLogger?.LogBestMove(searchResult.BestMove);
        return searchResult;
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
        _moveFinder.Reset();
    }

    public void SetPosition(Position position)
    {
        _game.ResetPosition(position);
    }

    public void SetThreads(int threads)
    {
        _engineSettings.SetThreads(threads);
        _moveFinder = new MoveFinder(_engineSettings,  _engineLogger);
    }

    public void SetHashSize(int hashSizeMb)
    {
        _engineSettings.SetTranspositionTableSizeMb(hashSizeMb);
    }
}
