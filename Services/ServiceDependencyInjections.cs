using Microsoft.Extensions.DependencyInjection;
using Services.BookCategories;
using Services.Words;
using Services.Books;
using Services.CheckingHistories;
using Services.Kids;
using Services.BookCategoryMappings;
using Services.CheckingHistories.Services;

namespace Services;

public static class ServiceDependencyInjections
{
    public static IServiceCollection Register(IServiceCollection services)
    {
        services.AddSingleton<IBookCategoryService, BookCategoryService>();
        services.AddSingleton<IKidService, KidService>();
        services.AddSingleton<ICheckingHistoryService, CheckingHistoryService>();
        services.AddSingleton<IWordHistoryBatchService, WordHistoryBatchService>();
        services.AddSingleton<IWordBatchService, WordBatchService>();
        services.AddSingleton<IWordPreCheckService, WordPreCheckService>();
        services.AddSingleton<IWordManageService, WordManageService>();
        services.AddSingleton<IBookService, BookService>();
        services.AddSingleton<IBookCategoryMappingService, BookCategoryMappingService>();

        return services;
    }
}
