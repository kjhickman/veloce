$ErrorActionPreference = 'Stop'

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
$worktreePath = Join-Path $repoRoot 'worktrees/baseline'

$status = & git -C $repoRoot status --porcelain
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
if ($status) {
    throw "Working tree is not clean. Commit or discard changes before promoting baseline."
}

& (Join-Path $PSScriptRoot 'ensure-baseline-worktree.ps1')
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$old = & git -C $repoRoot rev-parse baseline
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
$new = & git -C $repoRoot rev-parse HEAD
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$baselineStatus = & git -C $worktreePath status --porcelain
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
if ($baselineStatus) {
    throw "Baseline worktree is not clean. Commit or discard changes in $worktreePath before promoting baseline."
}

& git -C $worktreePath reset --hard $new
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

Write-Host "Promoted baseline: $old -> $new"
