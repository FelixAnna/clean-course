using Services.CheckingHistories.Models;

namespace Services.CheckingHistories.Services;

public interface IWordPreCheckService
{
    Task<SearchWordAndHistoryResult> GetAllAsync(int kidId, int bookCategoryId);
    void Remove(int kidId, int id);
    void Add(int kidId, CheckingHistoryModel word);
}
