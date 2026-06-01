# Agent Instructions

## Project Goal

Veloce is a UCI chess engine. The current workflow is designed for small search and evaluation iterations measured against the last accepted baseline with fastchess SPRT.

## Commands

Run fast checks for all code changes:

```bash
just test
just compliance
```

For search or evaluation changes, run a dirty SPRT before committing:

```bash
just sprt-dirty
```

For a shorter exploratory run:

```bash
just sprt-dirty-rounds 200
```

After committing a candidate, run the reproducible SPRT:

```bash
just sprt
```

Only promote the baseline when explicitly instructed:

```bash
just promote-baseline
```

## SPRT Setup

The SPRT script uses:

- opening book: `data/openings/8moves_v3.pgn`
- opening plies: `16`
- time control: `30+0.3`
- max moves: `80`
- SPRT: `elo0=0 elo1=5 alpha=0.05 beta=0.05`

Interpretation rule:

- `candidate` is engine 1.
- `baseline` is engine 2.
- `H1 accepted` means the candidate is stronger.
- `H0 accepted` means the candidate is not accepted.

## Baseline Rules

The `baseline` branch represents the last accepted engine.

Do not move or promote `baseline` unless explicitly asked.

Do not compare against stale artifacts. SPRT should publish fresh binaries for both candidate and baseline.

Dirty SPRT results are useful for iteration, but not reproducible. Important results should be rerun after committing.

## Generated Files

Do not commit:

- `artifacts/`
- `worktrees/`
- `config.json`
- build output under `bin/` or `obj/`

Fastchess may generate `config.json` after interrupted runs. It is disposable.

## Code Guidelines

Keep search changes small and measurable.

Prefer simple implementations before adding infrastructure.

For search or evaluation changes, add at least one focused test when the behavior can be expressed with a simple position.

## Verification Checklist

Before reporting success:

```bash
just test
just compliance
```

For strength changes, also report:

```bash
just sprt-dirty
```

or explain why SPRT was not run.
