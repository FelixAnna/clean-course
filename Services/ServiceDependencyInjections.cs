using Microsoft.Extensions.DependencyInjection;
using Services.BookCategories;
using Services.BookCategoryWords;
using Services.CheckingHistories;
using Services.CheckingHistories.Services;
using Services.Kids;
using Services.WordAndHistory;
using Services.WordAndHistory.Services;

namespace Services;

public static class ServiceDependencyInjections
{
    public static IServiceCollection Register(IServiceCollection services)
    {
        services.AddSingleton<IBookCategoryService, BookCategoryService>();
        services.AddSingleton<IKidService, KidService>();
        services.AddSingleton<IWordService, WordService>();
        services.AddSingleton<ICheckingHistoryService, CheckingHistoryService>();
        services.AddSingleton<IWordHistoryService, WordHistoryService>();
        services.AddSingleton<IWordImportService, WordImportService>();
        services.AddSingleton<IWordPreCheckService, WordPreCheckService>();

        return services;
    }
}
