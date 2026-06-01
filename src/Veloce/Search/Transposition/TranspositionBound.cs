namespace Veloce.Search.Transposition;

internal enum TranspositionBound : byte
{
    None = 0,
    Exact = 1,
    Lower = 2,
    Upper = 3,
}
