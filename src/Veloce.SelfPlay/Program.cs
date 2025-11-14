using System.Diagnostics;
using Veloce.Engine;
using Veloce.Search;

var engine1Settings = new EngineSettings
{
    MaxDepth = 99,
    TranspositionTableSizeMb = 256,
    Threads = 1,
};

var engine2Settings = new EngineSettings
{
    MaxDepth = 99,
    TranspositionTableSizeMb = 256,
    Threads = Environment.ProcessorCount,
};

var gameCount = 0;
var engine1Wins = 0;
var engine2Wins = 0;
const int timeControlMs = 2 * 60 * 1000; // 2 minutes per side
const int incrementMs = 1000; // 1 second increment

while (true)
{
    var engine1 = new VeloceEngine(engine1Settings);
    var engine2 = new VeloceEngine(engine2Settings);

    gameCount++;
    var moveCount = 0;

    // Alternate colors each game
    var engine1IsWhite = gameCount % 2 == 1;
    var whiteToMove = true;

    var engine1TimeMs = timeControlMs;
    var engine2TimeMs = timeControlMs;

    Console.WriteLine($"Starting game {gameCount}");
    Console.WriteLine($"Engine 1 ({(engine1IsWhite ? "White" : "Black")}): {engine1Settings.TranspositionTableSizeMb}MB TT, {engine1Settings.Threads} threads");
    Console.WriteLine($"Engine 2 ({(engine1IsWhite ? "Black" : "White")}): {engine2Settings.TranspositionTableSizeMb}MB TT, {engine2Settings.Threads} threads");
    Console.WriteLine($"Time control: {timeControlMs / 1000} seconds per side + {incrementMs / 1000} second increment");
    Console.WriteLine($"Current score - Engine 1: {engine1Wins}, Engine 2: {engine2Wins}");

    var gameStart = Stopwatch.StartNew();

    while (true)
    {
        moveCount++;
        var currentEngine = whiteToMove == engine1IsWhite ? engine1 : engine2;
        var currentTimeMs = whiteToMove == engine1IsWhite ? engine1TimeMs : engine2TimeMs;
        var currentEngineNumber = whiteToMove == engine1IsWhite ? 1 : 2;
        var playerName = $"Engine {currentEngineNumber} ({(whiteToMove ? "White" : "Black")})";

        if (currentTimeMs <= 0)
        {
            var winner = currentEngineNumber == 1 ? 2 : 1;
            if (winner == 1)
                engine1Wins++;
            else
                engine2Wins++;

            Console.WriteLine($"{playerName} loses on time! Engine {winner} wins! Game {gameCount} over after {moveCount - 1} moves.");
            Console.WriteLine($"Score update - Engine 1: {engine1Wins}, Engine 2: {engine2Wins}");
            break;
        }

        // Pass total remaining time - let engine calculate time to use
        var timeControl = new TimeControl(currentTimeMs, incrementMs);

        var moveStart = Stopwatch.StartNew();
        SearchResult searchResult;

        try
        {
            searchResult = currentEngine.FindBestMove(timeControl);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"EXCEPTION in {playerName}: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            Console.WriteLine($"Engine {currentEngineNumber} crashed! Program will exit.");
            throw;
        }

        var moveTime = (int)moveStart.ElapsedMilliseconds;

        // Update time remaining (subtract move time, add increment)
        if (whiteToMove == engine1IsWhite)
            engine1TimeMs = engine1TimeMs - moveTime + incrementMs;
        else
            engine2TimeMs = engine2TimeMs - moveTime + incrementMs;

        var remainingTimeSeconds = (whiteToMove == engine1IsWhite ? engine1TimeMs : engine2TimeMs) / 1000.0;
        var timeUsedSeconds = moveTime / 1000.0;
        var evalScore = searchResult.Score / 100.0;
        var nodesPerSecond = moveTime > 0 ? (long)(searchResult.NodesSearched * 1000.0 / moveTime) : 0;
        
        Console.WriteLine($"Game {gameCount}, Move {moveCount} ({playerName}): " +
                         $"Time used: {timeUsedSeconds:F1}s, Remaining: {remainingTimeSeconds:F1}s, " +
                         $"NPS: {nodesPerSecond:N0}, Eval: {evalScore:F2}, Move: {searchResult.BestMove}");

        if (searchResult.BestMove == null)
        {
            var result = whiteToMove ? "0-1 (Black wins)" : "1-0 (White wins)";
            var winner = whiteToMove ? engine1IsWhite ? 2 : 1 : engine1IsWhite ? 1 : 2;

            if (winner == 1)
                engine1Wins++;
            else
                engine2Wins++;

            Console.WriteLine($"Game {gameCount} over: {result} - Engine {winner} wins! After {moveCount - 1} moves. Total time: {gameStart.Elapsed}");
            Console.WriteLine($"Score update - Engine 1: {engine1Wins}, Engine 2: {engine2Wins}");
            break;
        }

        // Make the move on both engines to keep them synchronized
        engine1.MakeMove(searchResult.BestMove.Value);
        engine2.MakeMove(searchResult.BestMove.Value);

        whiteToMove = !whiteToMove;
    }

    Console.WriteLine("---");
}
