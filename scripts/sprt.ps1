param(
    [switch]$Dirty,
    [int]$Rounds = 10000,
    [int]$Concurrency = 0
)

$ErrorActionPreference = 'Stop'

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')

if ($Concurrency -le 0) {
    $Concurrency = [Environment]::ProcessorCount
}

if (-not $Dirty) {
    $status = & git -C $repoRoot status --porcelain
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    if ($status) {
        throw "Working tree is not clean. Commit changes or run 'just sprt-dirty'."
    }
}

& (Join-Path $PSScriptRoot 'ensure-baseline-worktree.ps1')
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$candidate = & (Join-Path $PSScriptRoot 'publish-engine.ps1') -Kind candidate | Select-Object -Last 1
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$baseline = & (Join-Path $PSScriptRoot 'publish-engine.ps1') -Kind baseline | Select-Object -Last 1
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

fastchess --compliance $candidate
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

fastchess --compliance $baseline
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$timestamp = Get-Date -Format 'yyyyMMdd-HHmmss'
$resultDir = Join-Path $repoRoot 'artifacts/results/sprt'
New-Item -ItemType Directory -Path $resultDir -Force | Out-Null
$pgn = Join-Path $resultDir "$timestamp.pgn"
$log = Join-Path $resultDir "$timestamp.log"
$openings = Join-Path $repoRoot 'data/openings/8moves_v3.pgn'
if (-not (Test-Path $openings)) {
    throw "Opening file does not exist: $openings"
}

if ($Dirty) {
    "Running dirty SPRT. Result is not reproducible until rerun from a commit." | Tee-Object -FilePath $log
}

$arguments = @(
    '-engine', "name=candidate", "cmd=$candidate", 'proto=uci',
    '-engine', "name=baseline", "cmd=$baseline", 'proto=uci',
    '-each', 'tc=30+0.3', 'option.Hash=16', 'option.Threads=1',
    '-rounds', "$Rounds",
    '-repeat',
    '-concurrency', "$Concurrency",
    '-openings', "file=$openings", 'format=pgn', 'order=random', 'plies=16',
    '-maxmoves', '80',
    '-sprt', 'elo0=0', 'elo1=5', 'alpha=0.05', 'beta=0.05',
    '-pgnout', "file=$pgn"
)

fastchess @arguments 2>&1 | Tee-Object -FilePath $log -Append
exit $LASTEXITCODE
