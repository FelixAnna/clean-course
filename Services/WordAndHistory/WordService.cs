using Services.WordAndHistory.Models;
using Services.WordAndHistory.Repositories;

namespace Services.WordAndHistory;

public class WordService(IWordRepository repository) : IWordService
{
    private readonly IWordRepository repository = repository;

    public async Task<SearchWordAndHistoryResult> GetWordsAsync(SearchWordAndHistoryCriteria request)
    {
        var words = await repository.FindAsync(request);

        var results = words.Select(x =>
        {
            return new WordModel(x);
        }).OrderBy(x => x.Course).ThenBy(x => x.Unit).ToList();

        return new SearchWordAndHistoryResult()
        {
            Words = results,
            Count = results.Count
        };
    }
}
