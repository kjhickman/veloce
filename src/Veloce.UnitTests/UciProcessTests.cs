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

        await Assert.That(Regex.IsMatch(bestMove, "^bestmove ([a-h][1-8][a-h][1-8][qrbn]?|0000)( ponder [a-h][1-8][a-h][1-8][qrbn]?)?$")).IsTrue();
        await Assert.That(process.ExitCode).IsEqualTo(0);
    }

    [Test]
    public async Task UciProcess_Uci_AdvertisesPolishOptions()
    {
        using var process = StartUciProcess();

        await SendLine(process, "uci");
        var output = await ReadUntilOutput(process, line => line == "uciok");

        await SendLine(process, "quit");
        await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(10));

        await Assert.That(output).Contains("option name Clear Hash type button");
        await Assert.That(output).Contains("option name MultiPV type spin default 1 min 1 max 1");
        await Assert.That(output).Contains("option name UCI_AnalyseMode type check default false");
        await Assert.That(process.ExitCode).IsEqualTo(0);
    }

    [Test]
    public async Task UciProcess_GoMoveTime_ReturnsBestMove()
    {
        using var process = StartUciProcess();

        await SendLine(process, "uci");
        await ReadUntil(process, line => line == "uciok");

        await SendLine(process, "position startpos");
        await SendLine(process, "go movetime 50");
        var bestMove = await ReadUntil(process, line => line.StartsWith("bestmove ", StringComparison.Ordinal));

        await SendLine(process, "quit");
        await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(10));

        await Assert.That(Regex.IsMatch(bestMove, "^bestmove ([a-h][1-8][a-h][1-8][qrbn]?|0000)( ponder [a-h][1-8][a-h][1-8][qrbn]?)?$")).IsTrue();
        await Assert.That(process.ExitCode).IsEqualTo(0);
    }

    [Test]
    public async Task UciProcess_GoDepth_EmitsSearchInfoBeforeBestMove()
    {
        using var process = StartUciProcess();

        await SendLine(process, "uci");
        await ReadUntil(process, line => line == "uciok");

        await SendLine(process, "position startpos");
        await SendLine(process, "go depth 2");
        var output = await ReadUntilBestMove(process);

        await SendLine(process, "quit");
        await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(10));

        var bestMoveIndex = output.FindIndex(line => line.StartsWith("bestmove ", StringComparison.Ordinal));
        var infoIndex = output.FindIndex(line => Regex.IsMatch(
            line,
            "^info depth 1 seldepth \\d+ multipv 1 score (cp -?\\d+|mate -?\\d+) nodes \\d+ nps \\d+ time \\d+ hashfull \\d+ pv [a-h][1-8][a-h][1-8][qrbn]?( [a-h][1-8][a-h][1-8][qrbn]?)*$"));

        await Assert.That(infoIndex).IsGreaterThanOrEqualTo(0);
        await Assert.That(infoIndex).IsLessThan(bestMoveIndex);
        await Assert.That(process.ExitCode).IsEqualTo(0);
    }

    [Test]
    public async Task UciProcess_SetOptionHash_RemainsReady()
    {
        using var process = StartUciProcess();

        await SendLine(process, "uci");
        await ReadUntil(process, line => line == "uciok");

        await SendLine(process, "setoption name Hash value 32");
        await SendLine(process, "setoption name Clear Hash");
        await SendLine(process, "setoption name MultiPV value 1");
        await SendLine(process, "setoption name UCI_AnalyseMode value true");
        await SendLine(process, "isready");
        await ReadUntil(process, line => line == "readyok");

        await SendLine(process, "quit");
        await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(10));

        await Assert.That(process.ExitCode).IsEqualTo(0);
    }

    [Test]
    public async Task UciProcess_SetOptionThreads_RemainsReadyAndSearches()
    {
        using var process = StartUciProcess();

        await SendLine(process, "uci");
        await ReadUntil(process, line => line == "uciok");

        await SendLine(process, "setoption name Threads value 2");
        await SendLine(process, "isready");
        await ReadUntil(process, line => line == "readyok");
        await SendLine(process, "position startpos");
        await SendLine(process, "go depth 2");
        var bestMove = await ReadUntil(process, line => line.StartsWith("bestmove ", StringComparison.Ordinal));

        await SendLine(process, "quit");
        await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(10));

        await Assert.That(Regex.IsMatch(bestMove, "^bestmove ([a-h][1-8][a-h][1-8][qrbn]?|0000)( ponder [a-h][1-8][a-h][1-8][qrbn]?)?$")).IsTrue();
        await Assert.That(process.ExitCode).IsEqualTo(0);
    }

    [Test]
    public async Task UciProcess_GoNodesPonder_ReturnsBestMove()
    {
        using var process = StartUciProcess();

        await SendLine(process, "uci");
        await ReadUntil(process, line => line == "uciok");

        await SendLine(process, "position startpos");
        await SendLine(process, "go ponder nodes 10");
        var bestMove = await ReadUntil(process, line => line.StartsWith("bestmove ", StringComparison.Ordinal));

        await SendLine(process, "quit");
        await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(10));

        await Assert.That(Regex.IsMatch(bestMove, "^bestmove ([a-h][1-8][a-h][1-8][qrbn]?|0000)( ponder [a-h][1-8][a-h][1-8][qrbn]?)?$")).IsTrue();
        await Assert.That(process.ExitCode).IsEqualTo(0);
    }

    [Test]
    public async Task UciProcess_GoInfiniteStop_ReturnsBestMove()
    {
        using var process = StartUciProcess();

        await SendLine(process, "uci");
        await ReadUntil(process, line => line == "uciok");

        await SendLine(process, "position startpos");
        await SendLine(process, "go infinite");
        await Task.Delay(50);
        await SendLine(process, "stop");
        var bestMove = await ReadUntil(process, line => line.StartsWith("bestmove ", StringComparison.Ordinal));

        await SendLine(process, "quit");
        await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(10));

        await Assert.That(Regex.IsMatch(bestMove, "^bestmove ([a-h][1-8][a-h][1-8][qrbn]?|0000)( ponder [a-h][1-8][a-h][1-8][qrbn]?)?$")).IsTrue();
        await Assert.That(process.ExitCode).IsEqualTo(0);
    }

    [Test]
    public async Task UciProcess_GoPerft_ReturnsRootCountsAndTotalNodes()
    {
        using var process = StartUciProcess();

        await SendLine(process, "uci");
        await ReadUntil(process, line => line == "uciok");

        await SendLine(process, "position startpos");
        await SendLine(process, "go perft 2");
        var output = await ReadUntilOutput(process, line => line == "Nodes searched: 400");

        await SendLine(process, "quit");
        await process.WaitForExitAsync().WaitAsync(TimeSpan.FromSeconds(10));

        await Assert.That(output).Contains("e2e4: 20");
        await Assert.That(output).Contains("Nodes searched: 400");
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

    private static async Task<List<string>> ReadUntilOutput(Process process, Func<string, bool> predicate)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
        var output = new List<string>();

        while (!cts.IsCancellationRequested)
        {
            var lineTask = process.StandardOutput.ReadLineAsync(cts.Token).AsTask();
            var line = await lineTask;
            if (line is null) break;

            output.Add(line);
            if (predicate(line)) return output;
        }

        var error = await process.StandardError.ReadToEndAsync(cts.Token);
        throw new TimeoutException($"Timed out waiting for UCI output. stderr: {error}");
    }

    private static async Task<List<string>> ReadUntilBestMove(Process process)
    {
        using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
        var output = new List<string>();

        while (!cts.IsCancellationRequested)
        {
            var lineTask = process.StandardOutput.ReadLineAsync(cts.Token).AsTask();
            var line = await lineTask;
            if (line is null) break;

            output.Add(line);
            if (line.StartsWith("bestmove ", StringComparison.Ordinal)) return output;
        }

        var error = await process.StandardError.ReadToEndAsync(cts.Token);
        throw new TimeoutException($"Timed out waiting for UCI bestmove. stderr: {error}");
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
