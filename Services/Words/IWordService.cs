using Services.Words.Models;

namespace Services.Words;

public interface IWordService
{
    Task<SearchWordsResult> GetWordsAsync(SearchWordsCriteria request);
    Task<SearchWordsResult> GetWordsForBookCategoryAsync(SearchWordsCriteria request);
    Task<bool> DeleteWordAsync(int wordId);
    Task<WordModel> UpdateWordAsync(int id, AddWordModel model);
    Task<WordModel> GetByIdAsync(int id);
}