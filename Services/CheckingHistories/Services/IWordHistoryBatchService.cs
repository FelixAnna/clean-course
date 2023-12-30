using Services.CheckingHistories.Models;
using Services.WordAndHistory;

namespace Services.CheckingHistories.Services
{
    public interface IWordHistoryBatchService
    {
        Task<string> ExportCheckingStatusAsync(ExportCheckingHistoryCriteria model);
        Task SaveCheckingStatusAsync(int wordId, int kidId, CheckingRemark status);
        Task<int> ImportHistoryAsync(ImportCheckingHistoryModel model);
        Task<IList<WordHistoryModel>> PreviewHistoryAsync(ImportCheckingHistoryModel model);
    }
}
