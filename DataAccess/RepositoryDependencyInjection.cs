using DataAccess.Repositories;
using Entities;
using Microsoft.Extensions.DependencyInjection;
using Services.BookCategories.Repositories;
using Services.CheckingHistories.Repositories;
using Services.Kids.Repositories;
using Services.Words;

namespace DataAccess;

public static class RepositoryDependencyInjection
{
    public static IServiceCollection Register(IServiceCollection services, Func<int> checkingThreshold, Func<int> recentThreshold)
    {
        services.AddSingleton<IBookCategoryRepository, BookCategoryRepository>();
        services.AddSingleton<IKidRepository, KidRepository>();
        services.AddSingleton<IWordRepository>(x => new WordRepository(x.GetRequiredService<AbstractCourseContext>(), checkingThreshold, recentThreshold));
        services.AddSingleton<ICheckingHistoryRepository, CheckingHistoryRepository>();

        return services;
    }
}
