#!/bin/bash

# This script adapts Veloce to work with perftree
# Parameters:
# $1 - depth
# $2 - FEN string
# $3 - optional space-separated list of moves

DEPTH=$1
FEN="$2"
MOVES="$3"

# Build the command based on whether moves are provided
if [ -z "$MOVES" ]; then
    # No moves provided, just run perft directly
    OUTPUT=$(dotnet run --project src/Veloce.Perft/Veloce.Perft.csproj "$DEPTH" "$FEN")
else
    # Moves provided - we need to handle this case
    # Check if your Veloce.Perft supports a moves parameter directly
    # If not, you might need to implement this functionality
    OUTPUT=$(dotnet run --project src/Veloce.Perft/Veloce.Perft.csproj "$DEPTH" "$FEN" "$MOVES")
fi

# Output the results
echo "$OUTPUT"
