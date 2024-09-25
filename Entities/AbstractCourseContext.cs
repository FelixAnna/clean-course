using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Entities;

public abstract class AbstractCourseContext : DbContext
{
    public DbSet<BookCategoryEntity> BookCategories { get; set; }

    public DbSet<BookEntity> Books { get; set; }
    public DbSet<BookCategoryMappingsEntity> BookCategoryMappings { get; set; }
    public DbSet<WordEntity> Words { get; set; }

    public DbSet<CheckingHistoryEntity> CheckingHistories { get; set; }

    public DbSet<KidEntity> Kids { get; set; }

    public AbstractCourseContext() { }

    public AbstractCourseContext(DbContextOptions options)
   : base(options)
    {
        Debug.WriteLine($"{ContextId} context created.");
    }

    // Dispose pattern.
    public override void Dispose()
    {
        Debug.WriteLine($"{ContextId} context disposed.");
        base.Dispose();
    }

    // Dispose pattern.
    public override ValueTask DisposeAsync()
    {
        Debug.WriteLine($"{ContextId} context disposed async.");
        return base.DisposeAsync();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .Entity<WordEntity>()
            .HasMany(e => e.CheckingHistories)
            .WithOne(e => e.Word)
            .OnDelete(DeleteBehavior.ClientCascade);

        modelBuilder
            .Entity<KidEntity>()
            .HasMany(e => e.CheckingHistories)
            .WithOne(e => e.Kid)
            .OnDelete(DeleteBehavior.ClientCascade);
    }
}
