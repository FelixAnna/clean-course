using Services.Words.Models;
using Services.Words.Repositories;

namespace Services.Words;

public class WordManageService(IWordRepository repository) : IWordManageService
{
    private readonly IWordRepository repository = repository;
    public async Task<SearchWordsResult> GetWordsForBookCategoryAsync(SearchWordsCriteria criteria)
    {
        var words = await repository.FindAsync(criteria);

        var results = words.Select(x =>
        {
            return new WordModel(x);
        }).OrderBy(x => x.BookId).ThenBy(x => x.Unit).ThenBy(x => x.Content).ToList();

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
