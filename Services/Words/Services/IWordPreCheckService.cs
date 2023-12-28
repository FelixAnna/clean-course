using Services.Words.Models;

namespace Services.Words.Services
{
    public interface IWordPreCheckService
    {
        SearchWordsResult GetAll(int kidId,string sharedCode);
        void Remove(int kidId, int id);
        void Add(int kidId, WordModel word);
    }
}
