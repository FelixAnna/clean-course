using Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.AzureSql;

public class SqlServerCourseContext : AbstractCourseContext
{
    public static readonly string CleanCourse = nameof(CleanCourse).ToLower();

    public SqlServerCourseContext() { }
    public SqlServerCourseContext(DbContextOptions<SqlServerCourseContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "#####";
        optionsBuilder.UseSqlServer(connectionString);
        base.OnConfiguring(optionsBuilder);
    }
}
