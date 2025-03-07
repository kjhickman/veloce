using System.Runtime.CompilerServices;
using Zugfish.Engine.Models;

namespace Zugfish.Engine.Extensions;

public static class CastlingRightsExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Contains(this CastlingRights rights, CastlingRights other) => (rights & other) != 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static CastlingRights Remove(this CastlingRights rights, CastlingRights other) => rights & ~other;
}
