using ChessLite.Parsing;
using ChessLite.State;
using Veloce.Engine;

namespace Veloce.Uci.Commands;

internal static class PositionCommand
{
    public static void Handle(VeloceEngine engine, string[] commandParts)
    {
        if (commandParts.Length < 2)
        {
            return;
        }

        var index = 1;
        Position position;

        if (IsToken(commandParts[index], "startpos"))
        {
            position = new Position();
            index++;
        }
        else if (IsToken(commandParts[index], "fen") && index + 1 < commandParts.Length)
        {
            var fenStart = ++index;

            while (index < commandParts.Length && !IsToken(commandParts[index], "moves"))
            {
                index++;
            }

            position = Fen.Parse(string.Join(' ', commandParts[fenStart..index]));
        }
        else
        {
            return;
        }

        engine.SetPosition(position);

        if (index >= commandParts.Length || !IsToken(commandParts[index], "moves"))
        {
            return;
        }

        index++;
        while (index < commandParts.Length)
        {
            try
            {
                engine.MakeUciMove(commandParts[index++]);
            }
            catch (ArgumentException)
            {
                break;
            }
        }
    }

    private static bool IsToken(string value, string expected)
    {
        return value.Equals(expected, StringComparison.OrdinalIgnoreCase);
    }
}
