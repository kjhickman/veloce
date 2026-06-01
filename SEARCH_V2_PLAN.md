# Search V2 Plan

## Goal

Rewrite Veloce around a very small, reliable engine loop, then iterate on playing strength using repeatable testing and fastchess SPRT.

The first version should contain no search logic. It should choose a random legal move using ChessLite, expose that through UCI, and establish the workflow for correctness checks, UCI compliance, and strength testing.

## Project Shape

Keep the useful solution structure, but remove search-era baggage.

Keep:

- `Veloce.slnx`
- `src/Veloce/Veloce.csproj`
- `src/Veloce.Uci/Veloce.Uci.csproj`
- a test project
- `.editorconfig`, `.gitignore`, license, README

Remove or replace:

- current `Search/*` implementation
- tactical puzzle tests that assume engine strength
- `Veloce.SelfPlay`
- old benchmark code unless needed later
- engine settings for hash, threads, transposition table, and search depth unless they are only accepted as ignored UCI options

Target layout:

```text
src/
  Veloce/
    Veloce.csproj
    VeloceEngine.cs
  Veloce.Uci/
    Veloce.Uci.csproj
    Program.cs
tests/
  Veloce.Tests/
    Veloce.Tests.csproj
scripts/
  ensure-baseline-worktree.ps1
  publish-engine.ps1
  compliance.ps1
  sprt.ps1
  promote-baseline.ps1
justfile
SEARCH_V2_PLAN.md
```

Generated files should live outside source paths:

```text
artifacts/
  engines/
    candidate/
    baseline/
  results/
    sprt/
worktrees/
  baseline/
```

Add these to `.gitignore`:

```gitignore
/artifacts/
/worktrees/
```

## Engine V2 Baseline

The initial engine should be deliberately boring:

- owns a ChessLite game/position
- generates legal moves through ChessLite
- chooses one legal move randomly
- returns no move only when the game has no legal moves
- has no evaluation
- has no search
- has no transposition table
- has no move ordering
- has no time management beyond responding quickly to `go`

The purpose is to make future strength changes measurable against a known-simple baseline.

## UCI Requirements

The UCI layer should be thin and robust.

Required commands:

- `uci`
- `isready`
- `ucinewgame`
- `position startpos`
- `position startpos moves ...`
- `position fen ...`
- `position fen ... moves ...`
- `go depth N`
- `go movetime N`
- `go wtime ... btime ... winc ... binc ...`
- `stop`
- `quit`
- `setoption name Threads value N`
- `setoption name Hash value N`

For the random-move version, `Threads` and `Hash` can be accepted and ignored. Accepting common options avoids fastchess friction.

The engine should always emit:

```text
bestmove <uci-move>
```

or, when there is no legal move:

```text
bestmove 0000
```

Avoid printing unknown-command noise to stdout during UCI sessions unless it is known to be safe for fastchess.

## Testing Layers

Use separate gates for correctness, protocol compliance, and strength.

### Unit And Integration Tests

Command:

```bash
just test
```

Should run:

```bash
dotnet test
```

Tests should cover:

- random engine returns legal moves from start position
- random engine returns legal moves from selected FEN positions
- UCI process responds to `uci` with `uciok`
- UCI process responds to `isready` with `readyok`
- UCI process responds to `go depth 1` with a syntactically valid `bestmove`

Keep these tests fast and deterministic where possible. If randomness is injectable, use a fixed seed in tests.

### Fastchess UCI Compliance

Command:

```bash
just compliance
```

Should publish the candidate engine, then run fastchess' built-in compliance check:

```bash
fastchess --compliance artifacts/engines/candidate/Veloce.Uci
```

Use the correct executable name for the current OS and publish mode.

This is the official UCI compliance smoke test. Keep the small transcript integration tests anyway because they run through `dotnet test` and catch simple regressions without requiring fastchess.

### SPRT Strength Testing

Commands:

```bash
just sprt
just sprt-dirty
```

`just sprt` should compare committed code only:

- require a clean working tree
- publish candidate from current `HEAD`
- publish baseline from the `baseline` branch worktree
- run candidate compliance
- optionally run baseline compliance
- run fastchess SPRT
- write PGN/log/results under `artifacts/results/sprt/`

`just sprt-dirty` should allow LLM/developer experimentation:

- publish candidate from the current working tree, including uncommitted changes
- publish baseline from the `baseline` branch worktree
- run the same fastchess flow
- clearly mark the result as dirty/non-reproducible

Important: a dirty SPRT result is useful for iteration, but important results should be rerun with `just sprt` after committing.

## Baseline Management

Use git branches and worktrees for source baselines. Do not commit engine binaries and do not keep a second source copy inside the main tree.

Canonical branches:

- `main`: current development/candidate source
- `baseline`: last accepted engine source

Worktree:

```bash
git worktree add worktrees/baseline baseline
```

Generated binaries:

- candidate binary: `artifacts/engines/candidate/`
- baseline binary: `artifacts/engines/baseline/`

This keeps history normal:

- the accepted engine is represented by a normal git commit
- diffs are easy with `git diff baseline..HEAD`
- SPRT compares source states, not mysterious copied binaries
- artifacts are disposable and gitignored

Promotion should be explicit:

```bash
just promote-baseline
```

Promotion should:

- require a clean working tree
- move the `baseline` branch to current `HEAD`
- update `worktrees/baseline`
- print the promoted commit hash
- never run automatically as part of SPRT

## justfile Interface

Use `just` as the public command surface and PowerShell for implementation logic.

Recommended recipes:

```just
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
```

Keep the `justfile` thin. Put path handling, timestamps, process exit handling, and git checks in PowerShell scripts.

## Script Responsibilities

### `scripts/ensure-baseline-worktree.ps1`

- ensure the `baseline` branch exists
- ensure `worktrees/baseline` exists
- ensure the worktree is on the `baseline` branch
- fail rather than overwrite local changes in the baseline worktree

### `scripts/publish-engine.ps1`

Parameters:

- `-Kind candidate|baseline`
- optional `-Dirty` if needed by caller

Responsibilities:

- publish `src/Veloce.Uci/Veloce.Uci.csproj`
- use Release configuration
- output to `artifacts/engines/<kind>/`
- publish candidate from repo root
- publish baseline from `worktrees/baseline`
- return the executable path to callers or write it in a predictable way

Start with the local runtime identifier, likely `osx-arm64`. Generalize later only if needed.

### `scripts/compliance.ps1`

- publish candidate
- run `fastchess --compliance <candidate-engine-path>`
- forward fastchess exit code

### `scripts/sprt.ps1`

Parameters:

- optional `-Dirty`

Responsibilities:

- ensure baseline worktree exists
- if not dirty mode, require clean working tree
- publish candidate
- publish baseline
- run candidate compliance
- optionally run baseline compliance
- run fastchess SPRT
- write timestamped PGN/log files
- print a concise result summary
- forward a meaningful exit code

Initial fastchess shape:

```bash
fastchess \
  -engine name=baseline cmd=artifacts/engines/baseline/Veloce.Uci proto=uci \
  -engine name=candidate cmd=artifacts/engines/candidate/Veloce.Uci proto=uci \
  -each tc=5+0.05 option.Hash=16 option.Threads=1 \
  -rounds 10000 \
  -repeat \
  -concurrency 4 \
  -openings file=data/openings/quiet-lanes.epd format=epd order=random \
  -sprt elo0=0 elo1=5 alpha=0.05 beta=0.05 \
  -pgnout file=artifacts/results/sprt/<timestamp>.pgn
```

Tune time control, concurrency, openings, and SPRT bounds after the random baseline is working.

### `scripts/promote-baseline.ps1`

- require clean working tree
- identify current `HEAD`
- move `baseline` branch to current `HEAD`
- update `worktrees/baseline`
- print old baseline and new baseline commit hashes
- fail rather than overwriting local changes in the baseline worktree

## LLM Workflow

Default improvement loop:

```bash
just test
just compliance
just sprt-dirty
```

For a result that should affect history:

```bash
git status
git diff
git commit -m "..."
just sprt
```

Promotion requires explicit instruction:

```bash
just promote-baseline
```

Rules for LLM agents:

- do not promote the baseline unless explicitly asked
- do not commit engine binaries
- do not edit files under `artifacts/` or `worktrees/` except through scripts
- run `just test` for all engine changes
- run `just compliance` for UCI changes
- run `just sprt-dirty` or `just sprt` for changes intended to improve playing strength
- report whether the SPRT was dirty or committed

## Implementation Order

1. Add `.gitignore` entries for generated artifacts and worktrees.
2. Strip `src/Veloce` to the minimal random legal move engine.
3. Rewrite `src/Veloce.Uci` as a thin, robust UCI loop.
4. Replace strength-dependent tests with legality and UCI transcript tests.
5. Add `justfile` and PowerShell scripts.
6. Create the initial `baseline` branch and worktree.
7. Verify `just test`.
8. Verify `just compliance`.
9. Verify `just sprt-dirty` against the baseline.
10. Start strength iterations.

## Success Criteria

The rewrite is ready for search iteration when:

- `just test` passes
- `just compliance` passes using `fastchess --compliance`
- `just sprt-dirty` can complete a candidate-vs-baseline run
- `just promote-baseline` can move the accepted baseline intentionally
- the initial engine has no search/eval code beyond random legal move selection
