using Microsoft.Extensions.DependencyInjection;
using Services.BookCategories;
using Services.BookCategoryWords;
using Services.CheckingHistories;
using Services.Kids;
using Services.WordAndHistory.Services;

namespace Services;

public static class ServiceDependencyInjections
{
    public static IServiceCollection Register(IServiceCollection services)
    {
        services.AddSingleton<IBookCategoryService, BookCategoryService>();
        services.AddSingleton<IKidService, KidService>();
        services.AddSingleton<WordAndHistory.IWordHistoryService, WordAndHistory.WordHistoryService>();
        services.AddSingleton<ICheckingHistoryService, CheckingHistoryService>();
        services.AddSingleton<CheckingHistories.Services.IWordHistoryBatchService, CheckingHistories.Services.WordHistoryBatchService>();
        services.AddSingleton<IWordBatchService, WordBatchService>();
        services.AddSingleton<IWordPreCheckService, WordPreCheckService>();
        services.AddSingleton<IWordManageService, WordManageService>();

        return services;
    }
}
