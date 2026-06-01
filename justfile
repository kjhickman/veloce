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

sprt-dirty:
    pwsh scripts/sprt.ps1 -Dirty

promote-baseline:
    pwsh scripts/promote-baseline.ps1
