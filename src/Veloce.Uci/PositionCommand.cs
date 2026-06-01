using ChessLite.Parsing;
using ChessLite.State;
using Veloce.Engine;

namespace Veloce.Uci;

internal static class PositionCommand
{
    public static void Handle(VeloceEngine engine, string[] commandParts)
    {
        if (commandParts.Length < 2) return;

        var index = 1;
        Position position;

        if (commandParts[index].Equals("startpos", StringComparison.OrdinalIgnoreCase))
        {
            position = new Position();
            index++;
        }
        else if (commandParts[index].Equals("fen", StringComparison.OrdinalIgnoreCase) && index + 1 < commandParts.Length)
        {
            var fenBuilder = new System.Text.StringBuilder();
            index++;

            while (index < commandParts.Length && !commandParts[index].Equals("moves", StringComparison.OrdinalIgnoreCase))
            {
                fenBuilder.Append(commandParts[index++]);
                fenBuilder.Append(' ');
            }

            position = Fen.Parse(fenBuilder.ToString().Trim());
        }
        else
        {
            return;
        }

        engine.SetPosition(position);

        if (index >= commandParts.Length || !commandParts[index].Equals("moves", StringComparison.OrdinalIgnoreCase)) return;

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
}
