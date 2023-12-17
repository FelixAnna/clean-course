using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Database.SQLite;

public static class SQLiteDependencyInjections
{
    public static IServiceCollection Register(IServiceCollection services, string courseDBFileName)
    {
        services.AddDbContextFactory<SQLiteCourseContext>(opt => opt.UseSqlite($"Data Source={courseDBFileName}"));
        services.AddSingleton<AbstractCourseContext, SQLiteCourseContext>();

        return services;
    }

}
