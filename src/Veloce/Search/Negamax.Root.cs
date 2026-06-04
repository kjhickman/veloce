using ChessLite;
using ChessLite.Movement;

namespace Veloce.Search;

public sealed partial class Negamax
{
    private (Move BestMove, int BestScore) SearchRootWithAspiration(
        Game game,
        Span<Move> moves,
        int moveCount,
        int depth,
        Move previousBestMove,
        int previousBestScore,
        Move rootTableMove,
        Move diversifiedRootMove,
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
                diversifiedRootMove,
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
        Move diversifiedRootMove,
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

            var move = PickNextRootMove(moves, moveCount, i, rootTableMove, diversifiedRootMove);
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
}
