using Services.Words.Models;

namespace Services.Words;

public class WordService(IWordRepository repository) : IWordService
{
    private readonly IWordRepository repository = repository;

    public async Task<SearchWordsResult> GetWordsAsync(SearchWordsCriteria request)
    {
        var words = await repository.FindAsync(request);

        var results = words.Select(x =>
        {
            return new WordModel(x);
        }).OrderBy(x => x.Course).ThenBy(x => x.Unit).ToList();

        return new SearchWordsResult()
        {
            Words = results,
            Count = results.Count
        };
    }
    public async Task<SearchWordsResult> GetWordsForBookCategoryAsync(SearchWordsCriteria request)
    {
        var words = await repository.FindAsync(request);

        var results = words.Select(x =>
        {
            return new WordModel(x);
        }).ToList();

        return new SearchWordsResult()
        {
            Words = results,
            Count = results.Count
        };
    }


    public async Task<WordModel> UpdateWordAsync(int id, AddWordModel model)
    {
        var result = await repository.UpdateAsync(id, model);
        return new WordModel(result);
    }
    public async Task<WordModel> GetByIdAsync(int id)
    {
        var result = await repository.GetByIdAsync(id);
        return new WordModel(result!);
    }

    public async Task<bool> DeleteWordAsync(int wordId)
    {
        return await repository.RemoveAsync(wordId);
    }
}
