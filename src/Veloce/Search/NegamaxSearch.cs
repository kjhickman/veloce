using System.Diagnostics;
using ChessLite;
using ChessLite.Movement;
using ChessLite.Primitives;
using Veloce.Engine;
using Veloce.Evaluation;
using Veloce.Search.Transposition;

namespace Veloce.Search;

public sealed class NegamaxSearch
{
    private const int MateScore = 100_000;
    private const int MateThreshold = MateScore - 1_000;
    private const int MaxQuiescencePly = 8;
    private const int MaxSearchPly = 128;
    private const int InitialAspirationWindow = 25;
    private const int MaxAspirationWindow = MateScore * 2;
    private const int NullMoveMinDepth = 3;
    private const int NullMoveReduction = 2;
    private const int TableMoveScore = 1_000_000;
    private const int CaptureMoveScore = 100_000;
    private const int PrimaryKillerScore = 90_000;
    private const int SecondaryKillerScore = 80_000;
    private const ulong HalfmoveHashMultiplier = 0x9E37_79B9_7F4A_7C15UL;
    private readonly TranspositionTable _transpositions = new();
    private readonly CompactMove[] _primaryKillers = new CompactMove[MaxSearchPly];
    private readonly CompactMove[] _secondaryKillers = new CompactMove[MaxSearchPly];
    private long _nodes;

    public void SetHashSize(int megabytes)
    {
        _transpositions.Resize(megabytes);
    }

    public SearchResult FindBestMove(
        Game game,
        SearchSettings settings,
        Action<SearchInfo>? searchInfo = null,
        CancellationToken cancellationToken = default)
    {
        var now = Stopwatch.GetTimestamp();
        _nodes = 0;
        Array.Clear(_primaryKillers);
        Array.Clear(_secondaryKillers);

        using var timeLimit = settings.MoveTime.HasValue ? new CancellationTokenSource(settings.MoveTime.Value) : null;
        using var linkedCancellation = timeLimit is null
            ? null
            : CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, timeLimit.Token);
        var effectiveCancellation = linkedCancellation?.Token ?? cancellationToken;

        _transpositions.NewSearch();

        if (game.GetDrawState().IsDraw())
        {
            return new SearchResult(null, 0, 0, _nodes, Stopwatch.GetElapsedTime(now));
        }

        Span<Move> moves = stackalloc Move[218];
        var moveCount = game.WriteLegalMoves(moves);
        if (moveCount == 0)
        {
            return new SearchResult(null, EvaluateTerminal(game, 0), 0, _nodes, Stopwatch.GetElapsedTime(now));
        }

        var bestMove = moves[0];
        var bestScore = MaterialEvaluator.Evaluate(game.Position);
        var completedDepth = 0;
        var rootKey = GetTranspositionKey(game);

        for (var depth = 1; depth <= settings.Depth; depth++)
        {
            try
            {
                effectiveCancellation.ThrowIfCancellationRequested();

                var rootTableMove = _transpositions.TryGet(rootKey, out var rootEntry)
                    ? rootEntry.Move.FindMatchingMove(moves, moveCount)
                    : Move.NullMove;
                var (depthBestMove, depthBestScore) = SearchRootWithAspiration(
                    game,
                    moves,
                    moveCount,
                    depth,
                    bestMove,
                    bestScore,
                    rootTableMove,
                    effectiveCancellation);

                bestMove = depthBestMove;
                bestScore = depthBestScore;
                completedDepth = depth;
                _transpositions.Store(rootKey, ScoreToTable(bestScore, 0), new CompactMove(bestMove), depth, TranspositionBound.Exact);
                searchInfo?.Invoke(new SearchInfo(bestMove, bestScore, completedDepth, _nodes, Stopwatch.GetElapsedTime(now)));
            }
            catch (OperationCanceledException) when (effectiveCancellation.IsCancellationRequested)
            {
                break;
            }
        }

        return new SearchResult(bestMove, bestScore, completedDepth, _nodes, Stopwatch.GetElapsedTime(now));
    }

    private (Move BestMove, int BestScore) SearchRootWithAspiration(
        Game game,
        Span<Move> moves,
        int moveCount,
        int depth,
        Move previousBestMove,
        int previousBestScore,
        Move rootTableMove,
        CancellationToken cancellationToken)
    {
        const int fullAlpha = int.MinValue + 1;
        const int fullBeta = int.MaxValue;
        var useAspiration = depth > 1 && Math.Abs(previousBestScore) < MateThreshold;
        var window = InitialAspirationWindow;
        var alpha = useAspiration ? Math.Max(fullAlpha, previousBestScore - window) : fullAlpha;
        var beta = useAspiration ? Math.Min(fullBeta, previousBestScore + window) : fullBeta;

        while (true)
        {
            var (bestMove, bestScore) = SearchRootDepth(
                game,
                moves,
                moveCount,
                depth,
                previousBestMove,
                rootTableMove,
                alpha,
                beta,
                cancellationToken);

            if (!useAspiration || (bestScore > alpha && bestScore < beta))
            {
                return (bestMove, bestScore);
            }

            if (bestScore <= alpha)
            {
                window = Math.Min(window * 2, MaxAspirationWindow);
                alpha = Math.Max(fullAlpha, previousBestScore - window);
            }
            else
            {
                window = Math.Min(window * 2, MaxAspirationWindow);
                beta = Math.Min(fullBeta, previousBestScore + window);
            }

            if (window == MaxAspirationWindow)
            {
                useAspiration = false;
                alpha = fullAlpha;
                beta = fullBeta;
            }
        }
    }

    private (Move BestMove, int BestScore) SearchRootDepth(
        Game game,
        Span<Move> moves,
        int moveCount,
        int depth,
        Move previousBestMove,
        Move rootTableMove,
        int alpha,
        int beta,
        CancellationToken cancellationToken)
    {
        var depthBestMove = previousBestMove;
        var depthBestScore = int.MinValue + 1;
        var searchedMoves = 0;

        for (var i = 0; i < moveCount; i++)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var move = PickNextMove(moves, moveCount, i, rootTableMove, 0, useKillers: false);
            game.MakeMove(move);
            int score;
            try
            {
                if (searchedMoves == 0)
                {
                    score = -Search(game, depth - 1, -beta, -alpha, 1, true, cancellationToken);
                }
                else
                {
                    score = -Search(game, depth - 1, -alpha - 1, -alpha, 1, true, cancellationToken);
                    if (score > alpha && score < beta)
                    {
                        score = -Search(game, depth - 1, -beta, -alpha, 1, true, cancellationToken);
                    }
                }
            }
            finally
            {
                game.UndoMove();
            }

            searchedMoves++;

            if (score > depthBestScore)
            {
                depthBestScore = score;
                depthBestMove = move;
            }

            if (score > alpha)
            {
                alpha = score;
            }

            if (alpha >= beta)
            {
                break;
            }
        }

        return (depthBestMove, depthBestScore);
    }

    private int Search(Game game, int depth, int alpha, int beta, int ply, bool allowNullMove, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _nodes++;

        if (game.GetDrawState().IsDraw())
        {
            return 0;
        }

        if (depth == 0)
        {
            return Quiescence(game, alpha, beta, ply, 0, cancellationToken);
        }

        var originalAlpha = alpha;
        var key = GetTranspositionKey(game);
        if (_transpositions.TryGet(key, out var entry))
        {
            if (entry.Depth >= depth)
            {
                var score = ScoreFromTable(entry.Score, ply);
                if (entry.Bound == TranspositionBound.Exact)
                {
                    return score;
                }

                if (entry.Bound == TranspositionBound.Lower && score >= beta)
                {
                    return score;
                }

                if (entry.Bound == TranspositionBound.Upper && score <= alpha)
                {
                    return score;
                }
            }
        }

        var inCheck = game.IsInCheck();
        if (ShouldTryNullMove(game, depth, alpha, beta, allowNullMove, inCheck))
        {
            game.MakeNullMove();
            int score;
            try
            {
                score = -Search(game, depth - 1 - NullMoveReduction, -beta, -beta + 1, ply + 1, false, cancellationToken);
            }
            finally
            {
                game.UndoMove();
            }

            if (score >= beta)
            {
                return beta;
            }
        }

        Span<Move> moves = stackalloc Move[218];
        var moveCount = game.WriteLegalMoves(moves);
        if (moveCount == 0)
        {
            return EvaluateTerminal(game, ply);
        }

        var tableMove = _transpositions.TryGet(key, out entry)
            ? entry.Move.FindMatchingMove(moves, moveCount)
            : Move.NullMove;

        var bestScore = int.MinValue + 1;
        var bestMove = Move.NullMove;
        var searchedMoves = 0;

        for (var i = 0; i < moveCount; i++)
        {
            var move = PickNextMove(moves, moveCount, i, tableMove, ply, useKillers: true);
            game.MakeMove(move);
            int score;
            try
            {
                if (searchedMoves == 0)
                {
                    score = -Search(game, depth - 1, -beta, -alpha, ply + 1, true, cancellationToken);
                }
                else
                {
                    score = -Search(game, depth - 1, -alpha - 1, -alpha, ply + 1, true, cancellationToken);
                    if (score > alpha && score < beta)
                    {
                        score = -Search(game, depth - 1, -beta, -alpha, ply + 1, true, cancellationToken);
                    }
                }
            }
            finally
            {
                game.UndoMove();
            }

            searchedMoves++;

            if (score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }

            if (score > alpha)
            {
                alpha = score;
            }

            if (alpha >= beta)
            {
                if (!move.IsCapture)
                {
                    StoreKiller(ply, move);
                }

                break;
            }
        }

        var bound = bestScore <= originalAlpha
            ? TranspositionBound.Upper
            : bestScore >= beta
                ? TranspositionBound.Lower
                : TranspositionBound.Exact;
        _transpositions.Store(key, ScoreToTable(bestScore, ply), new CompactMove(bestMove), depth, bound);

        return bestScore;
    }

    private static bool ShouldTryNullMove(Game game, int depth, int alpha, int beta, bool allowNullMove, bool inCheck)
    {
        return allowNullMove
            && depth >= NullMoveMinDepth
            && !inCheck
            && alpha > -MateThreshold
            && beta < MateThreshold
            && HasNonPawnMaterial(game);
    }

    private static bool HasNonPawnMaterial(Game game)
    {
        var position = game.Position;
        var pieces = position.WhiteToMove
            ? position.WhiteKnights | position.WhiteBishops | position.WhiteRooks | position.WhiteQueens
            : position.BlackKnights | position.BlackBishops | position.BlackRooks | position.BlackQueens;

        return pieces.IsNotEmpty();
    }

    private int Quiescence(Game game, int alpha, int beta, int ply, int qPly, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _nodes++;

        if (game.GetDrawState().IsDraw())
        {
            return 0;
        }

        if (qPly >= MaxQuiescencePly)
        {
            return MaterialEvaluator.Evaluate(game.Position);
        }

        var inCheck = game.IsInCheck();
        if (!inCheck)
        {
            var standPat = MaterialEvaluator.Evaluate(game.Position);
            if (standPat >= beta)
            {
                return beta;
            }

            if (standPat > alpha)
            {
                alpha = standPat;
            }
        }

        Span<Move> moves = stackalloc Move[218];
        var moveCount = game.WriteLegalMoves(moves);
        if (moveCount == 0)
        {
            return EvaluateTerminal(game, ply);
        }

        for (var i = 0; i < moveCount; i++)
        {
            var move = PickNextMove(moves, moveCount, i, Move.NullMove, ply, useKillers: false);
            if (!inCheck && !move.IsCapture)
            {
                continue;
            }

            game.MakeMove(move);
            int score;
            try
            {
                score = -Quiescence(game, -beta, -alpha, ply + 1, qPly + 1, cancellationToken);
            }
            finally
            {
                game.UndoMove();
            }

            if (score > alpha)
            {
                alpha = score;
            }

            if (alpha >= beta)
            {
                break;
            }
        }

        return alpha;
    }

    private static int EvaluateTerminal(Game game, int ply)
    {
        return game.IsInCheck() ? -MateScore + ply : 0;
    }

    private static ulong GetTranspositionKey(Game game)
    {
        var key = game.Position.ZobristHash ^ ((ulong)game.Position.HalfmoveClock * HalfmoveHashMultiplier);
        return key == 0 ? 1 : key;
    }

    private static int ScoreToTable(int score, int ply)
    {
        if (score > MateThreshold) return score + ply;
        if (score < -MateThreshold) return score - ply;
        return score;
    }

    private static int ScoreFromTable(int score, int ply)
    {
        if (score > MateThreshold) return score - ply;
        if (score < -MateThreshold) return score + ply;
        return score;
    }

    private Move PickNextMove(Span<Move> moves, int moveCount, int startIndex, Move tableMove, int ply, bool useKillers)
    {
        var bestIndex = startIndex;
        var bestScore = ScoreMove(moves[startIndex], tableMove, ply, useKillers);

        for (var i = startIndex + 1; i < moveCount; i++)
        {
            var score = ScoreMove(moves[i], tableMove, ply, useKillers);
            if (score > bestScore)
            {
                bestScore = score;
                bestIndex = i;
            }
        }

        (moves[startIndex], moves[bestIndex]) = (moves[bestIndex], moves[startIndex]);
        return moves[startIndex];
    }

    private int ScoreMove(Move move, Move tableMove, int ply, bool useKillers)
    {
        if (move == tableMove)
        {
            return TableMoveScore;
        }

        if (move.IsCapture)
        {
            return CaptureMoveScore + GetPieceValue(move.CapturedPieceType) * 16 - GetPieceValue(move.PieceType);
        }

        if (!useKillers || ply >= MaxSearchPly)
        {
            return 0;
        }

        var compactMove = new CompactMove(move);
        if (compactMove == _primaryKillers[ply])
        {
            return PrimaryKillerScore;
        }

        if (compactMove == _secondaryKillers[ply])
        {
            return SecondaryKillerScore;
        }

        return 0;
    }

    private void StoreKiller(int ply, Move move)
    {
        if (ply >= MaxSearchPly)
        {
            return;
        }

        var compactMove = new CompactMove(move);
        if (compactMove == _primaryKillers[ply])
        {
            return;
        }

        _secondaryKillers[ply] = _primaryKillers[ply];
        _primaryKillers[ply] = compactMove;
    }

    private static int GetPieceValue(PieceType pieceType)
    {
        return pieceType switch
        {
            PieceType.WhitePawn or PieceType.BlackPawn => 100,
            PieceType.WhiteKnight or PieceType.BlackKnight => 320,
            PieceType.WhiteBishop or PieceType.BlackBishop => 330,
            PieceType.WhiteRook or PieceType.BlackRook => 500,
            PieceType.WhiteQueen or PieceType.BlackQueen => 900,
            PieceType.WhiteKing or PieceType.BlackKing => 20_000,
            _ => 0,
        };
    }
}
