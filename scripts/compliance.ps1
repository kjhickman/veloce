$ErrorActionPreference = 'Stop'

$enginePath = & (Join-Path $PSScriptRoot 'publish-engine.ps1') -Kind candidate | Select-Object -Last 1
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

fastchess --compliance $enginePath
exit $LASTEXITCODE
