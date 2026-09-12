using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Database.SQLite;

public static class SQLiteDependencyInjections
{
    public static IServiceCollection Register(IServiceCollection services, string courseDBFileName)
    {
        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

        services.AddDbContext<SQLiteCourseContext>(opt => opt.UseSqlite($"Data Source={courseDBFileName}"));
        services.AddScoped<AbstractCourseContext>(x => x.GetRequiredService<SQLiteCourseContext>());

        return services;
    }

}
