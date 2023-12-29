using Services.BookCategoryWords.Models;

namespace Services.BookCategoryWords
{
    public interface IWordImportService
    {
        Task<WordModel> AddWordAsync(AddWordModel model);
        Task<IList<WordModel>> ImportWordsAsync(ImportWordsModel model);
        Task<IList<WordModel>> PreviewWordsAsync(ImportWordsModel model);


        Task<SearchWordsResult> GetWordsForBookCategoryAsync(SearchWordsCriteria criteria);
        Task<bool> DeleteWordAsync(int wordId);
        Task<WordModel> UpdateWordAsync(int id, AddWordModel model);
        Task<WordModel> GetByIdAsync(int id);
    }
}
