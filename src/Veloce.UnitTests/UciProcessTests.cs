using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Veloce.UnitTests;

public class UciProcessTests
{
    [Test]
    public async Task UciProcess_RespondsToBasicTranscript()
    {
        using var process = StartUciProcess();

        await SendLine(process, "uci");
        await ReadUntil(process, line => line == "uciok");

        await SendLine(process, "isready");
        await ReadUntil(process, line => line == "readyok");

        await SendLine(process, "ucinewgame");
        await SendLine(process, "position startpos moves e2e4 e7e5");
        await SendLine(process, "go depth 1");
        var bestMove = await ReadUntil(process, line => line.StartsWith("bestmove ", StringComparison.Ordinal));

        await SendLine(process, "quit");
        await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(10));

        await Assert.That(Regex.IsMatch(bestMove, "^bestmove ([a-h][1-8][a-h][1-8][qrbn]?|0000)$")).IsTrue();
        await Assert.That(process.ExitCode).IsEqualTo(0);
    }

    private static Process StartUciProcess()
    {
        var repoRoot = FindRepoRoot();
        var projectPath = Path.Combine(repoRoot, "src", "Veloce.Uci", "Veloce.Uci.csproj");
        var startInfo = new ProcessStartInfo("dotnet", $"run --project \"{projectPath}\" --no-restore")
        {
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            WorkingDirectory = repoRoot,
        };

        return Process.Start(startInfo) ?? throw new InvalidOperationException("Failed to start UCI process.");
    }

    private static async Task SendLine(Process process, string line)
    {
        await process.StandardInput.WriteLineAsync(line);
        await process.StandardInput.FlushAsync();
    }

    private static async Task<string> ReadUntil(Process process, Func<string, bool> predicate)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));

        while (!cts.IsCancellationRequested)
        {
            var lineTask = process.StandardOutput.ReadLineAsync(cts.Token).AsTask();
            var line = await lineTask;
            if (line is null) break;
            if (predicate(line)) return line;
        }

        var error = await process.StandardError.ReadToEndAsync(cts.Token);
        throw new TimeoutException($"Timed out waiting for UCI output. stderr: {error}");
    }

    private static string FindRepoRoot()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            if (File.Exists(Path.Combine(directory.FullName, "Veloce.slnx"))) return directory.FullName;
            directory = directory.Parent;
        }

        throw new DirectoryNotFoundException("Could not find repository root.");
    }
}
