using DataAccess.Repositories;
using Entities;
using Microsoft.Extensions.DependencyInjection;
using Services.BookCategories.Repositories;
using Services.BookCategoryMappings.Repository;
using Services.Books.Repositories;
using Services.CheckingHistories.Repositories;
using Services.Kids.Repositories;
using Services.Words.Repositories;

namespace DataAccess;

public static class RepositoryDependencyInjection
{
    public static IServiceCollection Register(IServiceCollection services, Func<IServiceProvider, int> checkingThreshold, Func<IServiceProvider, int> recentThreshold)
    {
        services.AddScoped<IBookCategoryRepository, BookCategoryRepository>();
        services.AddScoped<IKidRepository, KidRepository>();
        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IBookCategoryMappingRepository, BookCategoryMappingRepository>();
        services.AddScoped<IWordRepository>(x => new WordRepository(
            x.GetRequiredService<AbstractCourseContext>(),
            () => checkingThreshold(x),
            () => recentThreshold(x)));
        services.AddScoped<ICheckingHistoryRepository, CheckingHistoryRepository>();

        return services;
    }
}
