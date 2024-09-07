using Services.BookCategories;
using Services.BookCategoryMappings;
using Services.WordAndHistory.Models;

namespace Services.WordAndHistory.Services
{
    public class WordPreCheckService : IWordPreCheckService
    {
        private readonly IBookCategoryMappingService _bookCategoryMappingService;
        public WordPreCheckService(IBookCategoryMappingService bookCategoryMappingService)
        {
            _bookCategoryMappingService = bookCategoryMappingService;
        }

        private static Dictionary<int, List<WordHistoryModel>> _preCheckWordsForKids = [];

        public async Task<SearchWordAndHistoryResult> GetAllAsync(int kidId, int bookCategoryId)
        {
            if (!_preCheckWordsForKids.ContainsKey(kidId))
            {
                _preCheckWordsForKids[kidId] = new List<WordHistoryModel>();
            }

            var books = await _bookCategoryMappingService.GetBookCategoryMappingsById(bookCategoryId);
            if (books?.LinkedBooks == null || !books.LinkedBooks.Any())
            {
                return new SearchWordAndHistoryResult()
                {
                    Words = new List<WordHistoryModel>(),
                    Count = 0
                };
            }

            var bookIds = books.LinkedBooks.Select(x=>x.BookId).ToList();
            var words = _preCheckWordsForKids[kidId].Where(x => bookIds.Contains(x.BookId) ).OrderByDescending(x => x.BookId).ThenBy(x => x.Unit).ThenBy(x => x.WordId).ToList();

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
