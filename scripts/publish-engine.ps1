param(
    [Parameter(Mandatory = $true)]
    [ValidateSet('candidate', 'baseline')]
    [string]$Kind
)

$ErrorActionPreference = 'Stop'

$repoRoot = Resolve-Path (Join-Path $PSScriptRoot '..')
$sourceRoot = $repoRoot
if ($Kind -eq 'baseline') {
    & (Join-Path $PSScriptRoot 'ensure-baseline-worktree.ps1')
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
    $sourceRoot = Join-Path $repoRoot 'worktrees/baseline'
}

if ($IsMacOS) {
    $rid = if ([System.Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture -eq 'Arm64') { 'osx-arm64' } else { 'osx-x64' }
} elseif ($IsWindows) {
    $rid = if ([System.Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture -eq 'Arm64') { 'win-arm64' } else { 'win-x64' }
} else {
    $rid = if ([System.Runtime.InteropServices.RuntimeInformation]::ProcessArchitecture -eq 'Arm64') { 'linux-arm64' } else { 'linux-x64' }
}

$project = Join-Path $sourceRoot 'src/Veloce.Uci/Veloce.Uci.csproj'
$output = Join-Path $repoRoot "artifacts/engines/$Kind"
New-Item -ItemType Directory -Path $output -Force | Out-Null

dotnet publish $project --configuration Release --runtime $rid --self-contained true --output $output
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

$exeName = if ($IsWindows) { 'Veloce.Uci.exe' } else { 'Veloce.Uci' }
$enginePath = Join-Path $output $exeName
if (-not (Test-Path $enginePath)) {
    throw "Published engine was not found: $enginePath"
}

$enginePath
