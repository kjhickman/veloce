# Veloce

A *temporarily* low-performance  chess engine written in C#

## Overview

Veloce aims to be a modern, high-performance chess engine implemented in C#. The engine is compatible with the Universal Chess Interface (UCI) protocol, allowing it to be used with most chess GUIs.

## Requirements

- .NET 10.0 SDK or later

## Project Structure

The solution is organized into a small engine core, a UCI executable, and tests:

- **Veloce**: Core engine library using ChessLite
- **Veloce.Uci**: UCI-compatible CLI
- **Veloce.UnitTests**: Unit and process-level smoke tests

## Development Commands

```bash
just test
just compliance
just sprt-dirty
just sprt
just promote-baseline
```

`just compliance` uses `fastchess --compliance`. `just sprt` compares the current committed `HEAD` against the `baseline` branch worktree. `just sprt-dirty` allows uncommitted candidate testing, but those results should be rerun from a commit before promotion.

## Acknowledgments

- For the inspiration to start this project, [this video](https://youtu.be/w4FFX_otR-4?si=gOWyYTxIoEBOXrBn) by [Bartek Spitza](https://github.com/bartekspitza)
- The chess programming community at the [Chess Programming Wiki](https://www.chessprogramming.org)
