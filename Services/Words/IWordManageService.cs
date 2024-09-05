using Services.Words.Models;

namespace Services.Words
{
    public interface IWordManageService
    {
        Task<SearchWordsResult> GetWordsForBookCategoryAsync(SearchWordsCriteria criteria);
        Task<bool> DeleteWordAsync(int wordId);
        Task<WordModel> UpdateWordAsync(int id, AddWordModel model);
        Task<WordModel> GetByIdAsync(int id);
    }
}
