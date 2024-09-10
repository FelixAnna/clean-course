using Services.CheckingHistories.Models;

namespace Services.CheckingHistories;

public interface ICheckingHistoryService
{
    Task<SearchWordAndHistoryResult> GetWordsAsync(SearchWordAndHistoryCriteria request);
    Task<CheckingHistoryModel> GetByWordAndKidAsync(int wordId, int kidId);
    Task<CheckingHistoryModel> AddAsync(AddCheckingHistoryModel model);
    Task<bool> DeleteAsync(int historyId);
}
