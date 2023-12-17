using Services.Words.Models;

namespace Services.Words.Services
{
    public interface IWordImportService
    {
        Task<WordModel> AddWordAsync(AddWordModel model);
        Task<IList<WordModel>> ImportWordsAsync(ImportWordsModel model);
        Task<IList<WordModel>> PreviewWordsAsync(ImportWordsModel model);
    }
}
