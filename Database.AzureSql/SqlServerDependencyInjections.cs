using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database.SQLite;

public static class SqlServerDependencyInjections
{
    public static IServiceCollection Register(IServiceCollection services, IConfiguration configuration)
    {
        var courseDBConnectionString = configuration.GetConnectionString("CourseDBAzureSql");

        services.AddDbContextFactory<SqlServerCourseContext>(opt => opt.UseSqlServer(courseDBConnectionString));
        services.AddSingleton<AbstractCourseContext, SqlServerCourseContext>();

        return services;
    }
}
