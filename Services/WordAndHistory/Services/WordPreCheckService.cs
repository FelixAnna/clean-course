using Services.WordAndHistory.Models;

namespace Services.WordAndHistory.Services
{
    public class WordPreCheckService : IWordPreCheckService
    {
        private static Dictionary<int, List<WordModel>> _preCheckWordsForKids = [];

        public SearchWordAndHistoryResult GetAll(int kidId, string sharedCode)
        {
            if (!_preCheckWordsForKids.ContainsKey(kidId))
            {
                _preCheckWordsForKids[kidId] = new List<WordModel>();
            }

            var words = _preCheckWordsForKids[kidId].Where(x=>x.SharedCode == sharedCode).ToList();

            return new SearchWordAndHistoryResult()
            {
                Words = words,
                Count = words.Count()
            };
        }

        public void Add(int kidId, WordModel word)
        {
            if (!_preCheckWordsForKids.ContainsKey(kidId))
            {
                _preCheckWordsForKids[kidId] = new List<WordModel>();
            }

            _preCheckWordsForKids[kidId].Add(word);
        }

        public void Remove(int kidId, int id)
        {
            if (_preCheckWordsForKids.ContainsKey(kidId))
            {
                _preCheckWordsForKids[kidId].RemoveAll(x=>x.WordId == id);
            }
        }
    }
}
