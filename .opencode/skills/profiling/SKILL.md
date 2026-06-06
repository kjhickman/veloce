---
name: profiling
description: Use when profiling Veloce with dotnet-trace, pvanalyze, .nettrace files, CPU stacks, call trees, GC, JIT, allocations, UCI search, perft, or engine NPS investigations.
---

# Profiling

Use this skill for .NET performance investigations in Veloce that need `dotnet-trace` collection or `pvanalyze` analysis of `.nettrace` files. Prefer it when the user mentions profiling, `.nettrace`, `dotnet-trace`, `pvanalyze`, CPU stacks, call trees, GC pauses, allocations, JIT cost, exceptions, SpeedScope, NPS, perft speed, or search hot paths.

Veloce's strength workflow is still SPRT-based. Use profiles to identify or explain hot paths, then validate search/evaluation changes with the repo workflow in `AGENTS.md`.

## Setup

Use the repo-local profiling tools from `dotnet-tools.json`:

```bash
dotnet tool restore
dotnet tool run dotnet-trace -- --version
dotnet tool run pvanalyze -- --help
```

Do not add profiling tools as package references to engine projects. With global tools, replace `dotnet tool run dotnet-trace --` with `dotnet-trace` and `dotnet tool run pvanalyze --` with `pvanalyze`.

Write traces outside source-controlled paths or into ignored artifact folders. Good locations in this repo:

```text
/tmp/opencode
artifacts/profiles
```

## Trace Collection

Publish the candidate first when profiling engine search, so the trace is not dominated by build or `dotnet run` startup costs:

```bash
just publish-candidate
```

Collect CPU samples from the thread benchmark harness:

```bash
dotnet tool run dotnet-trace -- collect --output artifacts/profiles/veloce-bench-threads.nettrace -- pwsh scripts/bench-threads.ps1 -EnginePath artifacts/engines/candidate/Veloce.Uci -Threads 1 -MoveTime 3000
```

Collect CPU samples from a direct UCI session by attaching to a running engine process:

```bash
artifacts/engines/candidate/Veloce.Uci
dotnet tool run dotnet-trace -- collect --process-id <PID> --output artifacts/profiles/veloce-search.nettrace
```

Then drive the engine with focused UCI commands while tracing:

```text
uci
isready
setoption name Hash value 64
setoption name Threads value 1
position startpos
go depth 8
quit
```

Use `go nodes <N>` or `go movetime <MS>` when the investigation needs a stable amount of work. Use `setoption name Threads value 1` unless the task is specifically about Lazy SMP or scaling.

Collect allocation events when allocation-by-type analysis is needed:

```bash
dotnet tool run dotnet-trace -- collect --providers "Microsoft-Windows-DotNETRuntime:0x200001:5" --output artifacts/profiles/veloce-alloc.nettrace -- pwsh scripts/bench-threads.ps1 -EnginePath artifacts/engines/candidate/Veloce.Uci -Threads 1 -MoveTime 3000
```

Collect verbose runtime events for GC, JIT, exception, and event analysis:

```bash
dotnet tool run dotnet-trace -- collect --providers "Microsoft-Windows-DotNETRuntime:0x4C14FCCBD:5" --output artifacts/profiles/veloce-runtime.nettrace -- pwsh scripts/bench-threads.ps1 -EnginePath artifacts/engines/candidate/Veloce.Uci -Threads 1 -MoveTime 3000
```

For Veloce hot-path profiling, prefer sustained search or perft workloads. Avoid drawing conclusions from traces dominated by MSBuild, test discovery, publish, UCI handshake, process startup, or very shallow searches.

## Analyze With pvanalyze

Always verify the trace first:

```bash
dotnet tool run pvanalyze -- info artifacts/profiles/veloce-search.nettrace
```

CPU hotspots:

```bash
dotnet tool run pvanalyze -- cpustacks artifacts/profiles/veloce-search.nettrace --top 20
dotnet tool run pvanalyze -- cpustacks artifacts/profiles/veloce-search.nettrace --group-by namespace --inclusive
dotnet tool run pvanalyze -- calltree artifacts/profiles/veloce-search.nettrace --hot-path
dotnet tool run pvanalyze -- calltree artifacts/profiles/veloce-search.nettrace --caller-callee "Negamax.Search"
```

GC and allocations:

```bash
dotnet tool run pvanalyze -- gcstats artifacts/profiles/veloce-runtime.nettrace
dotnet tool run pvanalyze -- gcstats artifacts/profiles/veloce-runtime.nettrace --timeline
dotnet tool run pvanalyze -- alloc artifacts/profiles/veloce-alloc.nettrace --top 20
```

JIT, exceptions, and events:

```bash
dotnet tool run pvanalyze -- jitstats artifacts/profiles/veloce-runtime.nettrace
dotnet tool run pvanalyze -- exceptions artifacts/profiles/veloce-runtime.nettrace
dotnet tool run pvanalyze -- events artifacts/profiles/veloce-runtime.nettrace --list
dotnet tool run pvanalyze -- events artifacts/profiles/veloce-runtime.nettrace --provider DotNETRuntime --limit 50
```

SpeedScope export:

```bash
dotnet tool run pvanalyze -- cpustacks artifacts/profiles/veloce-search.nettrace --format speedscope --output artifacts/profiles/veloce-search.speedscope.json
```

Use `--format json` when another tool or agent will consume the result. Use `--from <ms>` and `--to <ms>` only after a baseline command identifies the interesting time window.

## Interpreting Results

For search CPU investigations, separate costs by area before proposing a change:

- Search control: `Veloce.Search.Negamax`, aspiration windows, reductions, pruning, node accounting.
- Move ordering: hash move, captures, killers, history, sorting/scoring loops.
- Evaluation: static eval, piece-square tables, material, terminal handling.
- Board operations: ChessLite move generation, make/unmake, attack checks, legality.
- Transposition table: probing, storing, hashfull, entry replacement, compact move conversion.

For throughput comparisons, report the workload shape as well as the profile. Include position source, depth/nodes/movetime, hash size, thread count, and whether the engine was published or run through `dotnet run`.

## Reporting

When reporting findings, include:

- The exact `dotnet-trace collect` command or the trace file path if the trace already existed.
- The focused `pvanalyze` commands used.
- The workload: UCI commands, position, depth, nodes, movetime, hash, and thread count.
- Top exclusive and inclusive CPU costs for CPU investigations.
- Allocation type totals or GC pause/timeline details for memory investigations.
- Any generated artifact paths, such as `.nettrace`, `.speedscope.json`, or `.pvanalyze.etlx` cache files.
- Whether the result suggests a code change, a benchmark follow-up, or an SPRT follow-up.

## Validation

Use the smallest validation that matches the work:

```bash
dotnet tool restore
dotnet tool run dotnet-trace -- --version
dotnet tool run pvanalyze -- --help
dotnet tool run pvanalyze -- info artifacts/profiles/veloce-search.nettrace
```

After code changes, follow the repo checklist:

```bash
just test
just compliance
```

For search or evaluation changes, also run a dirty SPRT or explain why it was not run:

```bash
just sprt-dirty
```

If JSON output is used, ensure it parses before relying on it for conclusions.
