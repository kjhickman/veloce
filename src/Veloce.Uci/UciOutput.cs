namespace Veloce.Uci;

internal sealed class UciOutput
{
    private readonly Lock _lock = new();

    public void WriteLine(string line)
    {
        lock (_lock)
        {
            Console.WriteLine(line);
        }
    }

    public void Flush()
    {
        lock (_lock)
        {
            Console.Out.Flush();
        }
    }
}
