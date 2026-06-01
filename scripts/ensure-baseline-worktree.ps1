$ErrorActionPreference = 'Stop'

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
$worktreePath = Join-Path $repoRoot 'worktrees/baseline'

function Invoke-Git {
    param([Parameter(ValueFromRemainingArguments = $true)][string[]]$Arguments)

    & git -C $repoRoot @Arguments
    if ($LASTEXITCODE -ne 0) {
        throw "git $($Arguments -join ' ') failed with exit code $LASTEXITCODE"
    }
}

& git -C $repoRoot rev-parse --verify baseline *> $null
if ($LASTEXITCODE -ne 0) {
    Invoke-Git @('branch', 'baseline', 'HEAD')
}

if (-not (Test-Path $worktreePath)) {
    New-Item -ItemType Directory -Path (Split-Path $worktreePath -Parent) -Force | Out-Null
    Invoke-Git @('worktree', 'add', $worktreePath, 'baseline')
    exit 0
}

$status = & git -C $worktreePath status --porcelain
if ($LASTEXITCODE -ne 0) {
    throw "Existing baseline worktree is not a git worktree: $worktreePath"
}

if ($status) {
    throw "Baseline worktree has local changes: $worktreePath"
}

& git -C $worktreePath checkout baseline
if ($LASTEXITCODE -ne 0) {
    throw "Failed to checkout baseline in $worktreePath"
}

& git -C $worktreePath reset --hard baseline
if ($LASTEXITCODE -ne 0) {
    throw "Failed to reset baseline worktree"
}
