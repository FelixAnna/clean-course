using Services.WordAndHistory.Models;

namespace Services.WordAndHistory;

public interface IWordHistoryService
{
    Task<SearchWordAndHistoryResult> GetWordsAsync(SearchWordAndHistoryCriteria request);
}