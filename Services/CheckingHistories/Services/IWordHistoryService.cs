using Services.CheckingHistories.Models;
using Services.WordAndHistory;

namespace Services.CheckingHistories.Services
{
    public interface IWordHistoryService
    {
        Task<string> ExportCheckingStatusAsync(ExportCheckingHistoryCriteria model);
        Task SaveCheckingStatusAsync(int wordId, int kidId, CheckingRemark status);
        Task<IList<CheckingHistory>> ImportHistoryAsync(ImportCheckingHistoryModel model);
        Task<IList<WordModel>> PreviewHistoryAsync(ImportCheckingHistoryModel model);
    }
}
