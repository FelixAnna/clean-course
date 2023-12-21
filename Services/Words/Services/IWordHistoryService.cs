using Services.CheckingHistories.Models;
using Services.Words.Models;

namespace Services.Words.Services
{
    public interface IWordHistoryService
    {
        Task<string> ExportCheckingStatusAsync(ExportWordHistoryModel model);
        Task SaveCheckingStatusAsync(int wordId, int kidId, CheckingRemark status);
        Task<IList<CheckingHistory>> ImportHistoryAsync(ImportWordHistoryModel model);
        Task<IList<WordModel>> PreviewHistoryAsync(ImportWordHistoryModel model);
    }
}
