using Services.BookCategoryMappings;
using Services.CheckingHistories.Models;

namespace Services.CheckingHistories.Services;

public class WordPreCheckService : IWordPreCheckService
{
    private readonly Dictionary<int, Dictionary<int, CheckingHistoryModel>> preCheckWordsByKid = [];

    private readonly IBookCategoryMappingService _bookCategoryMappingService;

    public WordPreCheckService(IBookCategoryMappingService bookCategoryMappingService)
    {
        _bookCategoryMappingService = bookCategoryMappingService;
    }
    
    public async Task<SearchWordAndHistoryResult> GetAllAsync(int kidId, int bookCategoryId)
    {
        var preCheckWords = GetPreCheckWords(kidId);

        var books = await _bookCategoryMappingService.GetByBookCategoryIdAsync(bookCategoryId);
        if (books?.LinkedBooks == null || !books.LinkedBooks.Any())
        {
            return new SearchWordAndHistoryResult()
            {
                Words = new List<CheckingHistoryModel>(),
                Count = 0
            };
        }

        var bookIds = books.LinkedBooks.Select(x=>x.BookId).ToList();
        var words = preCheckWords.Values.Where(x => bookIds.Contains(x.BookId)).OrderByDescending(x => x.BookId).ThenBy(x => x.Unit).ThenBy(x => x.WordId).ToList();

        return new SearchWordAndHistoryResult()
        {
            Words = words,
            Count = words.Count()
        };
    }

    public void Add(int kidId, CheckingHistoryModel word)
    {
        GetPreCheckWords(kidId)[word.WordId] = word;
    }

    public void Remove(int kidId, int id)
    {
        if (preCheckWordsByKid.TryGetValue(kidId, out var preCheckWords))
        {
            preCheckWords.Remove(id);
        }
    }

    private Dictionary<int, CheckingHistoryModel> GetPreCheckWords(int kidId)
    {
        if (!preCheckWordsByKid.TryGetValue(kidId, out var words))
        {
            words = [];
            preCheckWordsByKid[kidId] = words;
        }

        return words;
    }
}
