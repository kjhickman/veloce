using Veloce.Engine;

namespace Veloce.Uci;

internal static class SetOptionCommand
{
    private const int MinHashMegabytes = 1;
    private const int MaxHashMegabytes = 1024;

    public static void Handle(VeloceEngine engine, string[] commandParts)
    {
        var nameIndex = Array.FindIndex(commandParts, part => part.Equals("name", StringComparison.OrdinalIgnoreCase));
        var valueIndex = Array.FindIndex(commandParts, part => part.Equals("value", StringComparison.OrdinalIgnoreCase));
        if (nameIndex < 0 || valueIndex < 0 || valueIndex + 1 >= commandParts.Length)
        {
            return;
        }

        var optionName = string.Join(' ', commandParts[(nameIndex + 1)..valueIndex]);
        if (!optionName.Equals("Hash", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (!int.TryParse(commandParts[valueIndex + 1], out var megabytes))
        {
            return;
        }

        engine.SetHashSize(Math.Clamp(megabytes, MinHashMegabytes, MaxHashMegabytes));
    }
}
