using Veloce.Engine;

namespace Veloce.Uci.Commands;

internal static class SetOptionCommand
{
    private const int MinHashMegabytes = 1;
    private const int MaxHashMegabytes = 1024;
    private const int MinThreads = 1;

    public static void Handle(VeloceEngine engine, string[] commandParts)
    {
        var nameIndex = FindTokenIndex(commandParts, "name");
        var valueIndex = FindTokenIndex(commandParts, "value");
        if (nameIndex < 0)
        {
            return;
        }

        var optionNameEnd = valueIndex < 0 ? commandParts.Length : valueIndex;
        var optionName = string.Join(' ', commandParts[(nameIndex + 1)..optionNameEnd]);
        if (optionName.Equals("Clear Hash", StringComparison.OrdinalIgnoreCase))
        {
            engine.ClearHash();
            return;
        }

        if (valueIndex < 0 || valueIndex + 1 >= commandParts.Length)
        {
            return;
        }

        if (optionName.Equals("Hash", StringComparison.OrdinalIgnoreCase))
        {
            if (!int.TryParse(commandParts[valueIndex + 1], out var megabytes))
            {
                return;
            }

            engine.SetHashSize(Math.Clamp(megabytes, MinHashMegabytes, MaxHashMegabytes));
            return;
        }

        if (optionName.Equals("Threads", StringComparison.OrdinalIgnoreCase))
        {
            if (!int.TryParse(commandParts[valueIndex + 1], out var threads))
            {
                return;
            }

            engine.SetThreadCount(Math.Clamp(threads, MinThreads, VeloceEngine.MaximumThreadCount));
            return;
        }

        if (optionName.Equals("MultiPV", StringComparison.OrdinalIgnoreCase)
            || optionName.Equals("UCI_AnalyseMode", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }
    }

    private static int FindTokenIndex(string[] commandParts, string token)
    {
        return Array.FindIndex(commandParts, part => part.Equals(token, StringComparison.OrdinalIgnoreCase));
    }
}
