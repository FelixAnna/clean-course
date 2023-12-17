using Services.CheckingHistories.Models;

namespace Services.CheckingHistories;

public interface ICheckingHistoryService
{
    Task<CheckingHistoryModel> GetByWordAndKidAsync(int wordId, int kidId);
    Task<CheckingHistoryModel> AddAsync(AddCheckingHistoryModel model);
    Task<bool> DeleteAsync(int historyId);
    Task<IEnumerable<CheckingHistory>> AddAsync(params AddCheckingHistoryModel[] models);
    Task RemoveAllByWordIdAsync(int kidId, params int[] wordIds);
}
