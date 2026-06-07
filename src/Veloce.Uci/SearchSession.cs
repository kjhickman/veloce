using Veloce.Engine;

namespace Veloce.Uci;

internal sealed class SearchSession(UciWriter output)
{
    private readonly Lock _stateLock = new();
    private readonly UciWriter _output = output;
    private Task? _activeSearch;
    private CancellationTokenSource? _activeSearchCancellation;
    private int _activeSearchId;

    public async ValueTask StartAsync(Func<CancellationToken, Action<SearchInfo>, SearchResult> search)
    {
        await StopAsync();

        lock (_stateLock)
        {
            _activeSearchCancellation = new CancellationTokenSource();
            var searchCancellation = _activeSearchCancellation;
            var searchId = ++_activeSearchId;
            _activeSearch = Task.Run(() => CompleteSearch(search, searchCancellation, searchId), CancellationToken.None);
        }
    }

    public async ValueTask StopAsync()
    {
        Task? activeSearch;
        lock (_stateLock)
        {
            _activeSearchCancellation?.Cancel();
            activeSearch = _activeSearch;
        }

        if (activeSearch is null)
        {
            return;
        }

        await activeSearch;
    }

    private void CompleteSearch(
        Func<CancellationToken, Action<SearchInfo>, SearchResult> search,
        CancellationTokenSource searchCancellation,
        int searchId)
    {
        try
        {
            var result = search(searchCancellation.Token, info =>
            {
                _output.WriteLine(UciFormatting.FormatSearchInfo(info));
                _output.Flush();
            });

            _output.WriteLine(UciFormatting.FormatSearchResult(result));

            _output.WriteLine(UciFormatting.FormatBestMove(result));
            _output.Flush();
        }
        finally
        {
            lock (_stateLock)
            {
                if (searchId == _activeSearchId && ReferenceEquals(searchCancellation, _activeSearchCancellation))
                {
                    _activeSearch = null;
                    _activeSearchCancellation = null;
                    searchCancellation.Dispose();
                }
            }
        }
    }
}
