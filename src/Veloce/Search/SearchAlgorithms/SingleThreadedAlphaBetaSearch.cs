using System.Diagnostics;
using Veloce.Core.Extensions;
using Veloce.Core.Models;
using Veloce.Engine;
using Veloce.Evaluation;
using Veloce.Movement;
using Veloce.State;

namespace Veloce.Search.SearchAlgorithms;

/// <summary>
/// Single-threaded alpha-beta search algorithm implementation
/// </summary>
public class SingleThreadedAlphaBetaSearch : ISearchAlgorithm
{
    private readonly TranspositionTable _transpositionTable;
    private readonly EngineSettings _settings;

    // Search state
    private volatile bool _shouldStop;
    private long _nodesSearched;
    private readonly int[] _searchDepthNodes = new int[100];
    private TimeSpan _searchTimeLimit;
    private long _searchStartTimeStamp;
    private CancellationToken _cancellationToken;

    public SingleThreadedAlphaBetaSearch(EngineSettings? settings = null)
    {
        _settings = settings ?? EngineSettings.Default;
        _transpositionTable = new TranspositionTable(_settings.TranspositionTableSizeMb);
    }

    public long NodesSearched => _nodesSearched;
    
    public Action<SearchInfo>? OnSearchInfoAvailable { get; set; }

    public bool ShouldStop 
    { 
        get => _shouldStop; 
        set => _shouldStop = value; 
    }

    public SearchResult FindBestMove(Game game, int maxDepth, int timeLimit, CancellationToken cancellationToken = default)
    {
        _shouldStop = false;
        _cancellationToken = cancellationToken;
        ResetSearchCounters();
        _searchStartTimeStamp = Stopwatch.GetTimestamp();
        _searchTimeLimit = timeLimit > 0 ? TimeSpan.FromMilliseconds(timeLimit) : TimeSpan.MaxValue;

        // Start a new search generation
        _transpositionTable.NewSearch();

        var result = new SearchResult();
        for (var depth = 1; depth <= maxDepth; depth++)
        {
            if (ShouldStopSearch())
            {
                break;
            }

            var iterationResult = SearchAtDepth(game, depth);

            var elapsedTime = Stopwatch.GetElapsedTime(_searchStartTimeStamp);
            var searchInfo = new SearchInfo
            {
                Depth = depth,
                Score = game.Position.WhiteToMove ? iterationResult.Score : -iterationResult.Score,
                IsMateScore = IsForcedMateScore(iterationResult.Score),
                NodesSearched = _nodesSearched,
                TimeElapsed = elapsedTime,
                NodesPerSecond = (long)(_nodesSearched / elapsedTime.TotalSeconds),
                HashFull = _transpositionTable.GetOccupancy(),
            };
            
            OnSearchInfoAvailable?.Invoke(searchInfo);

            if (!_shouldStop)
            {
                result = iterationResult;
                result.Depth = depth;
                result.NodesSearched = _nodesSearched;
                result.TimeElapsed = elapsedTime;
            }

            if (_shouldStop || IsForcedMateScore(iterationResult.Score))
            {
                break;
            }
        }

        return result;
    }

    public void Reset()
    {
        _transpositionTable.Clear();
        _shouldStop = false;
        _nodesSearched = 0;
        Array.Clear(_searchDepthNodes, 0, _searchDepthNodes.Length);
    }

    private void ResetSearchCounters()
    {
        _nodesSearched = 0;
        Array.Clear(_searchDepthNodes, 0, _searchDepthNodes.Length);
    }

    private bool ShouldStopSearch()
    {
        return _shouldStop || 
               _cancellationToken.IsCancellationRequested || 
               Stopwatch.GetElapsedTime(_searchStartTimeStamp) > _searchTimeLimit;
    }

    private SearchResult SearchAtDepth(Game game, int depth)
    {
        Span<Move> movesBuffer = stackalloc Move[218];
        var moveCount = MoveGeneration.GenerateLegalMoves(game.Position, movesBuffer);

        if (moveCount == 0)
        {
            // Checkmate or stalemate
            var gameState = game.IsInCheck() ? GameState.Checkmate : GameState.Stalemate;
            var score = gameState == GameState.Checkmate
                ? game.Position.WhiteToMove ? -10000 : 10000
                : 0;
            return new SearchResult
            {
                BestMove = null,
                Score = score,
                GameState = gameState,
            };
        }

        var position = game.Position;
        var ttMove = Move.NullMove;
        if (_transpositionTable.Probe(position.ZobristHash, out var ttCompactMove, out _, out _, out _, out _))
        {
            if (ttCompactMove != 0) // Check if not a null move
            {
                ttMove = ttCompactMove.FindMatchingMove(movesBuffer, moveCount);
            }
        }

        OrderMoves(movesBuffer, moveCount, ttMove);

        var isMaximizing = position.WhiteToMove;
        var bestMove = movesBuffer[0];
        var bestScore = position.WhiteToMove ? int.MinValue : int.MaxValue;
        var alpha = int.MinValue;
        var beta = int.MaxValue;

        for (var i = 0; i < moveCount; i++)
        {
            if (ShouldStopSearch())
            {
                _shouldStop = true;
                break;
            }

            var move = movesBuffer[i];
            game.MakeMove(move);
            var score = AlphaBeta(game, depth - 1, alpha, beta, !isMaximizing, 1).Score;
            game.UndoMove();

            if (isMaximizing)
            {
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }

                alpha = Math.Max(alpha, score);
            }
            else
            {
                if (score < bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }

                beta = Math.Min(beta, score);
            }
        }

        if (!_shouldStop)
        {
            _transpositionTable.Store(
                position.ZobristHash,
                bestMove.ToCompactMove(),
                (short)bestScore,
                (short)position.Evaluate(),
                (byte)depth,
                TranspositionNodeType.Exact);
        }

        return new SearchResult
        {
            BestMove = bestMove,
            Score = bestScore,
            GameState = GameState.Ongoing,
        };
    }

    private EvaluationResult AlphaBeta(Game game, int depth, int alpha, int beta,
        bool isMaximizing, int ply)
    {
        _nodesSearched++;
        _searchDepthNodes[depth]++;

        if ((_nodesSearched & 4095) == 0 && ShouldStopSearch())
        {
            _shouldStop = true;
            return new EvaluationResult(0, GameState.Ongoing);
        }

        if (game.IsDrawByFiftyMoves())
        {
            return new EvaluationResult(0, GameState.DrawFiftyMove);
        }

        if (game.IsDrawByInsufficientMaterial())
        {
            return new EvaluationResult(0, GameState.DrawInsufficientMaterial);
        }

        if (game.IsDrawByRepetition())
        {
            return new EvaluationResult(0, GameState.DrawRepetition);
        }

        // Transposition table lookup
        var ttHit = false;
        var ttMove = Move.NullMove;
        var position = game.Position;
        if (_transpositionTable.Probe(position.ZobristHash, out var ttCompactMove, out var ttScore, out var ttEval, out var ttDepth, out var ttBound))
        {
            ttHit = true;

            // Only use TT entries if their depth is sufficient
            if (ttDepth >= depth)
            {
                // We can use the TT score if:
                // 1. It's an exact score, or
                // 2. It's a beta bound and score >= beta, or
                // 3. It's an alpha bound and score <= alpha
                switch (ttBound)
                {
                    case TranspositionNodeType.Exact:
                    case TranspositionNodeType.Beta when ttScore <= alpha:
                    case TranspositionNodeType.Alpha when ttScore >= beta:
                        return new EvaluationResult(ttScore, GameState.Ongoing);
                }
            }
        }

        // Check for leaf nodes
        Span<Move> movesBuffer = stackalloc Move[218];
        var moveCount = MoveGeneration.GenerateLegalMoves(position, movesBuffer);

        if (moveCount == 0)
        {
            return game.IsInCheck()
                ? new EvaluationResult(isMaximizing ? -10000 + ply : 10000 - ply, GameState.Checkmate)
                : new EvaluationResult(0, GameState.Stalemate);
        }

        if (depth <= 0)
        {
            // Reached depth limit, evaluate the position
            return new EvaluationResult(position.Evaluate(), GameState.Ongoing);
        }

        // Move ordering - try the TT move first if we have one
        if (ttHit && ttCompactMove != 0)
        {
            ttMove = ttCompactMove.FindMatchingMove(movesBuffer, moveCount);
        }
        OrderMoves(movesBuffer, moveCount, ttMove);

        var originalAlpha = alpha;
        var bestScore = isMaximizing ? int.MinValue : int.MaxValue;
        var bestMove = Move.NullMove;

        // Main search loop
        for (var i = 0; i < moveCount; i++)
        {
            if (_shouldStop) break;

            var move = movesBuffer[i];
            game.MakeMove(move);
            var score = AlphaBeta(game, depth - 1, alpha, beta, !isMaximizing, ply + 1).Score;
            game.UndoMove();

            if (isMaximizing)
            {
                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
                alpha = Math.Max(alpha, score);
            }
            else
            {
                if (score < bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
                beta = Math.Min(beta, score);
            }

            if (beta <= alpha) break;
        }

        // Don't store anything if we had to abort
        if (_shouldStop)
        {
            return new EvaluationResult(bestScore, GameState.Ongoing);
        }

        // Save this position to the transposition table
        var nodeType = bestScore <= originalAlpha ? TranspositionNodeType.Beta :
                       bestScore >= beta ? TranspositionNodeType.Alpha :
                       TranspositionNodeType.Exact;

        _transpositionTable.Store(
            position.ZobristHash,
            bestMove.ToCompactMove(),
            (short)bestScore,
            (short)position.Evaluate(), // Evaluate position for static evaluation
            (byte)depth,
            nodeType);

        return new EvaluationResult(bestScore, GameState.Ongoing);
    }

    private bool IsForcedMateScore(int score)
    {
        // Detect forced mate scores (allowing some buffer for mate distance)
        return Math.Abs(score) > 9000;
    }

    /// <summary>
    /// Orders the moves in descending order according to a simple heuristic.
    /// Moves matching the TT best move are given a huge bonus; capture moves are scored using MVV-LVA;
    /// promotion moves are also boosted.
    /// </summary>
    private void OrderMoves(Span<Move> moves, int moveCount, Move ttMove)
    {
        // Compute a score for each move.
        Span<int> scores = stackalloc int[moveCount];
        for (var i = 0; i < moveCount; i++)
        {
            scores[i] = ScoreMove(moves[i], ttMove);
        }

        // A simple quadratic sort on the small move list.
        for (var i = 0; i < moveCount - 1; i++)
        {
            for (var j = i + 1; j < moveCount; j++)
            {
                if (scores[j] <= scores[i]) continue;

                (moves[i], moves[j]) = (moves[j], moves[i]);
                (scores[i], scores[j]) = (scores[j], scores[i]);
            }
        }
    }

    /// <summary>
    /// Returns a score for the move. A higher score means the move is expected to be better.
    /// </summary>
    private int ScoreMove(Move move, Move ttMove)
    {
        // Transposition table best move gets the highest score.
        if (move.Equals(ttMove))
        {
            return 1000000;
        }

        var score = 0;
        if (move.IsCapture)
        {
            // MVV-LVA: bonus = (captured value - mover value) plus a base bonus.
            var captured = move.CapturedPieceType;
            var mover = move.PieceType;
            score = GetPieceValue(captured) - GetPieceValue(mover) + 10000;
        }
        else if (move.PromotedPieceType != PromotedPieceType.None)
        {
            // Give a bonus based on the promotion piece (promotion to queen is best)
            switch (move.PromotedPieceType)
            {
                case PromotedPieceType.Queen:
                    score = 900;
                    break;
                case PromotedPieceType.Rook:
                    score = 500;
                    break;
                case PromotedPieceType.Bishop:
                    score = 330;
                    break;
                case PromotedPieceType.Knight:
                    score = 320;
                    break;
                default:
                    score = 0;
                    break;
            }
            score += 800; // extra bonus for promotion moves
        }

        return score;
    }

    /// <summary>
    /// Returns a basic material value for a piece type.
    /// </summary>
    private int GetPieceValue(PieceType piece)
    {
        switch (piece)
        {
            case PieceType.WhitePawn:
            case PieceType.BlackPawn:
                return 100;
            case PieceType.WhiteKnight:
            case PieceType.BlackKnight:
                return 320;
            case PieceType.WhiteBishop:
            case PieceType.BlackBishop:
                return 330;
            case PieceType.WhiteRook:
            case PieceType.BlackRook:
                return 500;
            case PieceType.WhiteQueen:
            case PieceType.BlackQueen:
                return 900;
            default:
                return 0;
        }
    }
}
