using Services.WordAndHistory.Models;

namespace Services.WordAndHistory.Services
{
    public class WordPreCheckService : IWordPreCheckService
    {
        private static Dictionary<int, List<WordHistoryModel>> _preCheckWordsForKids = [];

        public SearchWordAndHistoryResult GetAll(int kidId, string sharedCode)
        {
            if (!_preCheckWordsForKids.ContainsKey(kidId))
            {
                _preCheckWordsForKids[kidId] = new List<WordHistoryModel>();
            }

            var words = _preCheckWordsForKids[kidId]/*.Where(x => x.BookId == sharedCode)*/.OrderByDescending(x => x.BookId).ThenBy(x => x.Unit).ThenBy(x => x.WordId).ToList();

            return new SearchWordAndHistoryResult()
            {
                Words = words,
                Count = words.Count()
            };
        }

        public void Add(int kidId, WordHistoryModel word)
        {
            if (!_preCheckWordsForKids.ContainsKey(kidId))
            {
                _preCheckWordsForKids[kidId] = new List<WordHistoryModel>();
            }

            _preCheckWordsForKids[kidId].Add(word);
        }

        public void Remove(int kidId, int id)
        {
            if (_preCheckWordsForKids.ContainsKey(kidId))
            {
                _preCheckWordsForKids[kidId].RemoveAll(x => x.WordId == id);
            }
        }
    }
}
