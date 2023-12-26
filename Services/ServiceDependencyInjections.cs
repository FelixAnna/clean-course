using Microsoft.Extensions.DependencyInjection;
using Services.BookCategories;
using Services.CheckingHistories;
using Services.CheckingHistories.Services;
using Services.Kids;
using Services.Words;
using Services.Words.Services;

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

        return services;
    }
}
