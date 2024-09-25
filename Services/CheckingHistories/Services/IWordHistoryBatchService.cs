using Services.CheckingHistories.Models.Admin;

namespace Services.CheckingHistories.Services;

public interface IWordHistoryBatchService
{
    Task<string> ExportCheckingStatusAsync(ExportCheckingHistoryCriteria model);
    Task<int> ImportHistoryAsync(ImportCheckingHistoryModel model);
    Task<IList<CheckingHistoryModel>> PreviewHistoryAsync(ImportCheckingHistoryModel model);
}
