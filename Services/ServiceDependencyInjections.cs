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
        services.AddScoped<IBookCategoryService, BookCategoryService>();
        services.AddScoped<IKidService, KidService>();
        services.AddScoped<ICheckingHistoryService, CheckingHistoryService>();
        services.AddScoped<IWordHistoryBatchService, WordHistoryBatchService>();
        services.AddScoped<IWordBatchService, WordBatchService>();
        services.AddScoped<IWordPreCheckService, WordPreCheckService>();
        services.AddScoped<IWordManageService, WordManageService>();
        services.AddScoped<IBookService, BookService>();
        services.AddScoped<IBookCategoryMappingService, BookCategoryMappingService>();

        return services;
    }
}
