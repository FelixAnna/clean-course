using Entities;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Books.Models;
using Services.Books.Repositories;

namespace DataAccess.Repositories;

public class BookRepository(AbstractCourseContext courseContext) : IBookRepository
{
    private readonly AbstractCourseContext courseContext = courseContext;

    public async Task<IEnumerable<BookEntity>> GetAllAsync()
    {
        return await courseContext.Books.ToListAsync();
    }

    public async Task<BookEntity?> GetByIdAsync(int bookId)
    {
        var book = await courseContext.Books.FindAsync(bookId);
        return book;
    }

    public async Task<BookEntity> AddAsync(AddBookModel model)
    {
        var result = await courseContext.Books.AddAsync(new BookEntity
        {
            BookName = model.BookName,
            AuditYear = model.AuditYear,
            Grade = model.Grade,
            Semester = model.Semester,
        });

        await courseContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<BookEntity> UpdateAsync(int bookId, AddBookModel model)
    {
        var book = await courseContext.Books.FirstAsync(x => x.BookId == bookId);
        book.BookName = model.BookName;
        book.AuditYear = model.AuditYear;
        book.Grade = model.Grade;
        book.Semester = model.Semester;
        await courseContext.SaveChangesAsync();
        return book;
    }

    public async Task<bool> RemoveAsync(int bookId)
    {
        var book = await courseContext.Books.Include(x => x.Words).FirstOrDefaultAsync(x => x.BookId == bookId);
        if (book != null)
        {
            courseContext.Books.Remove(book);
            await courseContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}
