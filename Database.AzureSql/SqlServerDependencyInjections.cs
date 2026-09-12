using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Database.AzureSql;

public static class SqlServerDependencyInjections
{
    public static IServiceCollection Register(IServiceCollection services, IConfiguration configuration)
    {
        var courseDBConnectionString = configuration.GetConnectionString("CourseDBAzureSql");

        services.AddDbContext<SqlServerCourseContext>(opt => opt.UseSqlServer(courseDBConnectionString));
        services.AddScoped<AbstractCourseContext>(x => x.GetRequiredService<SqlServerCourseContext>());

        return services;
    }
}
