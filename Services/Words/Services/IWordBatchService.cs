using Services.Words.Models;

namespace Services.Words
{
    public interface IWordBatchService
    {
        Task<WordModel> AddWordAsync(AddWordModel model);
        Task<IList<WordModel>> ImportWordsAsync(ImportWordsModel model);
        Task<IList<WordModel>> PreviewWordsAsync(ImportWordsModel model);
    }
}
