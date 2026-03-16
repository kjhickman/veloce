# Veloce

A *temporarily* low-performance  chess engine written in C#

## Overview

Veloce aims to be a modern, high-performance chess engine implemented in C#. The engine is compatible with the Universal Chess Interface (UCI) protocol, allowing it to be used with most chess GUIs.

## Requirements

- .NET 10.0 SDK or later

## Project Structure

The solution is organized into several projects:

- **Veloce**: Core engine library with move generation, evaluation, and search
- **Veloce.Uci**: UCI-compliant CLI
- **Veloce.Perft**: Performance/correctness testing tool
- **Veloce.SelfPlay**: Temporary self-play testing application
- **Veloce.UnitTests**: Unit tests
- **Veloce.Benchmarks**: Performance benchmarks

## Acknowledgments

- For the inspiration to start this project, [this video](https://youtu.be/w4FFX_otR-4?si=gOWyYTxIoEBOXrBn) by [Bartek Spitza](https://github.com/bartekspitza)
- The chess programming community at the [Chess Programming Wiki](https://www.chessprogramming.org)
