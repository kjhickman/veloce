// ReSharper disable InconsistentNaming

using System.Runtime.CompilerServices;

namespace Zugfish.Engine.Models;

//  noWe        north         noEa
//         +7    +8    +9
//             \  |  /
// west    -1 <-  0 -> +1    east
//             /  |  \
//         -9    -8    -7
// soWe         south        soEa
public enum Square
{
    a1, b1, c1, d1, e1, f1, g1, h1,
    a2, b2, c2, d2, e2, f2, g2, h2,
    a3, b3, c3, d3, e3, f3, g3, h3,
    a4, b4, c4, d4, e4, f4, g4, h4,
    a5, b5, c5, d5, e5, f5, g5, h5,
    a6, b6, c6, d6, e6, f6, g6, h6,
    a7, b7, c7, d7, e7, f7, g7, h7,
    a8, b8, c8, d8, e8, f8, g8, h8,
    None = -1
}

public static class SquareExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Bitboard ToMask(this Square square) => Bitboard.Mask(square);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetRank(this Square square) => (int)square / 8;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int GetFile(this Square square) => (int)square % 8;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsValid(this Square square) => Enum.IsDefined(square) && square != Square.None;
    // TODO: benchmark vs >0 and <64
}
