using Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.SQLite;

public class SQLiteCourseContext : AbstractCourseContext
{
    public static readonly string CleanCourse = nameof(CleanCourse).ToLower();

    public SQLiteCourseContext() { }
    public SQLiteCourseContext(DbContextOptions<SQLiteCourseContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //optionsBuilder.UseSqlite($@"Data Source=C:\Users\yuecn\Desktop\home\{nameof(CleanCourse)}.db"); 
        base.OnConfiguring(optionsBuilder);
    }
}
