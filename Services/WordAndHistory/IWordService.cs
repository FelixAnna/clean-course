using Services.WordAndHistory.Models;

namespace Services.WordAndHistory;

public interface IWordService
{
    Task<SearchWordAndHistoryResult> GetWordsAsync(SearchWordAndHistoryCriteria request);
}