using Microsoft.Extensions.DependencyInjection;
using Services.BookCategories;
using Services.Words;
using Services.Books;
using Services.CheckingHistories;
using Services.Kids;
using Services.WordAndHistory.Services;
using Services.BookCategoryMappings;

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
        services.AddSingleton<IBookService, BookService>();
        services.AddSingleton<IBookCategoryMappingService, BookCategoryMappingService>();

        return services;
    }
}
