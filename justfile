default:
    @just --list --unsorted

test:
    dotnet test

publish-candidate:
    pwsh scripts/publish-engine.ps1 -Kind candidate

publish-baseline:
    pwsh scripts/publish-engine.ps1 -Kind baseline

compliance:
    pwsh scripts/compliance.ps1

sprt:
    pwsh scripts/sprt.ps1

sprt-rounds rounds:
    pwsh scripts/sprt.ps1 -Rounds {{rounds}}

sprt-threads threads:
    pwsh scripts/sprt.ps1 -EngineThreads {{threads}}

sprt-dirty:
    pwsh scripts/sprt.ps1 -Dirty

sprt-dirty-rounds rounds:
    pwsh scripts/sprt.ps1 -Dirty -Rounds {{rounds}}

sprt-dirty-threads threads:
    pwsh scripts/sprt.ps1 -Dirty -EngineThreads {{threads}}

sprt-dirty-rounds-threads rounds threads:
    pwsh scripts/sprt.ps1 -Dirty -Rounds {{rounds}} -EngineThreads {{threads}}

promote-baseline:
    pwsh scripts/promote-baseline.ps1
