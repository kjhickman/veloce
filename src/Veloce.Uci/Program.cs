using Veloce.Core;
using Veloce.Engine;
using Veloce.State;
using Veloce.Uci.Lib;

namespace Veloce.Uci;

public static class Program
{
    public static void Main()
    {
        var logger = new UciEngineLogger();
        var settings = EngineSettings.Default;
        var engine = new VeloceEngine(settings, logger);
        while (true)
        {
            var line = Console.ReadLine()?.Trim().ToLower();
            if (string.IsNullOrEmpty(line)) continue;

            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var command = parts[0];

            switch (command)
            {
                case "uci":
                    Console.WriteLine("id name Veloce");
                    Console.WriteLine("id author Kyle Hickman");
                    Console.WriteLine("option name Threads type spin default 1 min 1 max 32");
                    Console.WriteLine("option name Hash type spin default 16 min 1 max 128");
                    Console.WriteLine("uciok");
                    break;

                case "setoption":
                    SetOption(engine, parts);
                    break;

                case "isready":
                    Console.WriteLine("readyok");
                    break;

                case "ucinewgame":
                    engine.NewGame();
                    break;

                case "position":
                    SetPosition(engine, parts);
                    break;

                case "go":
                    var timeControl = ParseTimeControl(parts);
                    engine.FindBestMove(timeControl);
                    break;

                case "stop":
                    // TODO: implement stop logic
                    break;

                case "quit":
                    return;

                default:
                    Console.WriteLine($"Unknown command: {command}");
                    break;
            }
        }
    }

    // TODO: Safely parse time control
    private static TimeControl ParseTimeControl(string[] parts)
    {
        var timeLeft = -1;
        var increment = -1;
        var movesToGo = -1;

        for (var i = 1; i < parts.Length; i++)
        {
            if (i + 1 >= parts.Length) break;

            switch (parts[i])
            {
                case "wtime":
                case "btime":
                    int.TryParse(parts[i + 1], out timeLeft);
                    i++;
                    break;

                case "winc":
                case "binc":
                    int.TryParse(parts[i + 1], out increment);
                    i++;
                    break;

                case "movestogo":
                    int.TryParse(parts[i + 1], out movesToGo);
                    i++;
                    break;

                case "depth":
                case "nodes":
                case "infinite":
                    return TimeControl.Infinite;
            }
        }

        return new TimeControl(timeLeft, increment, movesToGo);
    }

    private static void SetOption(VeloceEngine veloceEngine, string[] commandParts)
    {
        // Expected format: setoption name <option_name> value <option_value>
        if (commandParts.Length < 5)
            return;

        var nameIndex = Array.IndexOf(commandParts, "name");
        var valueIndex = Array.IndexOf(commandParts, "value");

        if (nameIndex == -1 || valueIndex == -1 || nameIndex >= valueIndex)
            return;

        // Extract option name (might be multiple words between "name" and "value")
        var optionNameParts = new List<string>();
        for (var i = nameIndex + 1; i < valueIndex; i++)
        {
            optionNameParts.Add(commandParts[i]);
        }
        var optionName = string.Join(" ", optionNameParts).ToLower();

        // Extract option value (everything after "value")
        var optionValueParts = new List<string>();
        for (var i = valueIndex + 1; i < commandParts.Length; i++)
        {
            optionValueParts.Add(commandParts[i]);
        }
        var optionValue = string.Join(" ", optionValueParts);

        // Handle specific options
        switch (optionName)
        {
            case "threads":
                if (int.TryParse(optionValue, out var threads))
                {
                    veloceEngine.SetThreads(threads);
                }
                break;

            case "hash":
                if (int.TryParse(optionValue, out var hashSize))
                {
                    veloceEngine.SetHashSize(hashSize);
                }
                break;
        }
    }

    private static void SetPosition(VeloceEngine veloceEngine, string[] commandParts)
    {
        if (commandParts.Length < 2)
            return;

        var index = 1; // Skip "position" command
        Position position;

        if (commandParts[index] == "startpos")
        {
            position = new Position();
            index++;
        }
        else if (commandParts[index] == "fen" && index + 1 < commandParts.Length)
        {
            // Parse FEN string - might span multiple array elements
            var fenBuilder = new System.Text.StringBuilder();
            index++; // Move past "fen"

            // Collect parts of the FEN string until we hit "moves" or end of command
            while (index < commandParts.Length && commandParts[index] != "moves")
            {
                fenBuilder.Append(commandParts[index++] + " ");
            }

            var fen = fenBuilder.ToString().Trim();
            position = new Position(fen);
        }
        else
        {
            // Invalid command format
            return;
        }

        veloceEngine.SetPosition(position);

        if (index >= commandParts.Length || commandParts[index] != "moves") return;

        index++; // Skip "moves" keyword
        // Apply each move
        while (index < commandParts.Length)
        {
            var moveUci = commandParts[index++];
            var move = Helpers.MoveFromUci(position, moveUci);

            if (move != Move.NullMove)
            {
                veloceEngine.MakeMove(move);
            }
            else
            {
                // Invalid move
                break;
            }
        }
    }
}
