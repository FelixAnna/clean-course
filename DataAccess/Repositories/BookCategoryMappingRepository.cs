using Entities;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Services.BookCategoryMappings.Repository;

namespace DataAccess.Repositories;

public class BookCategoryMappingRepository(AbstractCourseContext courseContext) : IBookCategoryMappingRepository
{
    private readonly AbstractCourseContext courseContext = courseContext;

    public async Task<IEnumerable<BookCategoryMappingsEntity?>> GetByBookCategoryIdAsync(int bookCategoryId)
    {
        var mappings = await courseContext.BookCategoryMappings
            .Include(x=>x.BookCategory)
            .Include(x=>x.Book)
            .Where(x=>x.BookCategoryId == bookCategoryId)
            .ToListAsync();
        return mappings;
    }

    public async Task<BookCategoryMappingsEntity> AddAsync(int bookCategoryId, int bookId)
    {
        var result = await courseContext.BookCategoryMappings.AddAsync(new BookCategoryMappingsEntity()
        {
            BookCategoryId=bookCategoryId,
            BookId=bookId
        });

        await courseContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<bool> RemoveAsync(int bookCategoryId, int bookId)
    {
        var mapping = await courseContext.BookCategoryMappings.FirstOrDefaultAsync(x => x.BookId == bookId && x.BookCategoryId == bookCategoryId);
        if (mapping != null)
        {
            courseContext.BookCategoryMappings.Remove(mapping);
            await courseContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}
