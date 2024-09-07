using Services.WordAndHistory.Models;

namespace Services.WordAndHistory.Services
{
    public interface IWordPreCheckService
    {
        Task<SearchWordAndHistoryResult> GetAllAsync(int kidId, int bookCategoryId);
        void Remove(int kidId, int id);
        void Add(int kidId, WordHistoryModel word);
    }
}
