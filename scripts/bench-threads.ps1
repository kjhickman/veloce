param(
    [string]$EnginePath = '',
    [string[]]$Threads = @('1', '4'),
    [int]$MoveTime = 1000,
    [int]$Hash = 16
)

$ErrorActionPreference = 'Stop'

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
if (-not $EnginePath) {
    $EnginePath = Join-Path $repoRoot 'artifacts/engines/candidate/Veloce.Uci'
}

if (-not (Test-Path $EnginePath)) {
    throw "Engine was not found: $EnginePath"
}

$positions = @(
    'startpos',
    'fen rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 2',
    'fen r1bqkbnr/pppp1ppp/2n5/4p3/4P3/5N2/PPPP1PPP/RNBQKB1R w KQkq - 2 3',
    'fen r2q1rk1/3n1pb1/pp1p1np1/3Pp2p/2P5/1N2BP2/PP1QB1PP/R4RK1 w - - 1 15',
    'fen 2r2rk1/1bqnbppp/p3pn2/1p1p4/3P4/1PN1PN2/PBQB1PPP/2R2RK1 w - - 0 12',
    'fen r3r1k1/pp3ppp/2n2n2/2bqp3/3p4/2PP1NP1/PP1NPPBP/R1BQ1RK1 w - - 0 10',
    'fen 4rrk1/1pp2ppp/p1nqbn2/3p4/3P4/2PBPN2/PPQN1PPP/R3R1K1 w - - 2 14',
    'fen 2rq1rk1/pp2bppp/2n1pn2/3p4/3P1B2/2PBPN2/PPQ2PPP/R3R1K1 b - - 4 12',
    'fen 8/2p5/3p1k2/1p1Pp3/1P2Pp2/2P2P2/6K1/8 w - - 0 42',
    'fen 6k1/5ppp/8/8/8/8/5PPP/6K1 w - - 0 1'
)

$threadCounts = foreach ($thread in $Threads) {
    foreach ($part in $thread -split ',') {
        if ($part.Trim()) {
            [int]$part.Trim()
        }
    }
}

function Start-Engine {
    $startInfo = [System.Diagnostics.ProcessStartInfo]::new($EnginePath)
    $startInfo.RedirectStandardInput = $true
    $startInfo.RedirectStandardOutput = $true
    $startInfo.RedirectStandardError = $true
    $startInfo.UseShellExecute = $false

    $process = [System.Diagnostics.Process]::Start($startInfo)
    if ($null -eq $process) {
        throw 'Failed to start engine.'
    }

    return $process
}

function Send-Line {
    param($Process, [string]$Line)

    $Process.StandardInput.WriteLine($Line)
    $Process.StandardInput.Flush()
}

function Read-Until {
    param($Process, [scriptblock]$Predicate)

    $deadline = [DateTime]::UtcNow.AddSeconds(30)
    while ([DateTime]::UtcNow -lt $deadline) {
        $line = $Process.StandardOutput.ReadLine()
        if ($null -eq $line) { break }
        if (& $Predicate $line) { return $line }
    }

    $errorText = $Process.StandardError.ReadToEnd()
    throw "Timed out waiting for engine output. stderr: $errorText"
}

function Run-Position {
    param($Process, [string]$Position, [int]$MoveTime)

    Send-Line $Process "position $Position"
    Send-Line $Process "go movetime $MoveTime"

    $lastInfo = ''
    $bestMove = ''
    while ($true) {
        $line = $Process.StandardOutput.ReadLine()
        if ($null -eq $line) { throw 'Engine exited before bestmove.' }

        if ($line.StartsWith('info ', [StringComparison]::Ordinal)) {
            $lastInfo = $line
        }

        if ($line.StartsWith('bestmove ', [StringComparison]::Ordinal)) {
            $bestMove = $line.Substring('bestmove '.Length)
            break
        }
    }

    if ($lastInfo -notmatch 'depth (?<depth>\d+) score cp (?<score>-?\d+) nodes (?<nodes>\d+) time (?<time>\d+)') {
        throw "Could not parse info line: $lastInfo"
    }

    $elapsed = [Math]::Max(1, [int64]$Matches.time)
    $nodes = [int64]$Matches.nodes
    [pscustomobject]@{
        Depth = [int]$Matches.depth
        Score = [int]$Matches.score
        Nodes = $nodes
        Time = $elapsed
        Nps = [int64]($nodes * 1000 / $elapsed)
        BestMove = $bestMove
    }
}

$allResults = @()
foreach ($threadCount in $threadCounts) {
    $process = Start-Engine
    try {
        Send-Line $process 'uci'
        Read-Until $process { param($line) $line -eq 'uciok' } | Out-Null
        Send-Line $process "setoption name Hash value $Hash"
        Send-Line $process "setoption name Threads value $threadCount"
        Send-Line $process 'isready'
        Read-Until $process { param($line) $line -eq 'readyok' } | Out-Null

        for ($i = 0; $i -lt $positions.Count; $i++) {
            $result = Run-Position $process $positions[$i] $MoveTime
            $allResults += [pscustomobject]@{
                Threads = $threadCount
                Position = $i + 1
                Depth = $result.Depth
                Score = $result.Score
                Nodes = $result.Nodes
                Time = $result.Time
                Nps = $result.Nps
                BestMove = $result.BestMove
            }
        }
    }
    finally {
        if (-not $process.HasExited) {
            Send-Line $process 'quit'
            $process.WaitForExit(5000) | Out-Null
        }

        $process.Dispose()
    }
}

$allResults | Format-Table -AutoSize

''
'Summary:'
$allResults |
    Group-Object Threads |
    ForEach-Object {
        $rows = $_.Group
        [pscustomobject]@{
            Threads = [int]$_.Name
            Positions = $rows.Count
            AvgDepth = [Math]::Round(($rows | Measure-Object Depth -Average).Average, 2)
            TotalNodes = ($rows | Measure-Object Nodes -Sum).Sum
            TotalTime = ($rows | Measure-Object Time -Sum).Sum
            AggregateNps = [int64]((($rows | Measure-Object Nodes -Sum).Sum) * 1000 / [Math]::Max(1, (($rows | Measure-Object Time -Sum).Sum)))
        }
    } |
    Format-Table -AutoSize
