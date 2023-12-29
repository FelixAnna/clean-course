using Services.WordAndHistory.Models;

namespace Services.WordAndHistory.Services
{
    public interface IWordPreCheckService
    {
        SearchWordAndHistoryResult GetAll(int kidId,string sharedCode);
        void Remove(int kidId, int id);
        void Add(int kidId, WordModel word);
    }
}
