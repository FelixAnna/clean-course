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
            Version = model.Version,
        });

        await courseContext.SaveChangesAsync();
        return result.Entity;
    }


    public async Task<IEnumerable<BookEntity>> FindAsync(string keywords)
    {
        var books = courseContext.Books.AsQueryable();

        if (!string.IsNullOrEmpty(keywords))
        {
            books = books.Where(x => x.BookName.Contains(keywords)
                                    || x.Semester.Contains(keywords)
                                    || x.Version.Contains(keywords)
                                    || x.AuditYear.ToString().Contains(keywords)
                                    || x.Grade.Contains(keywords));
        }

        return await books.ToListAsync();
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
            var words = courseContext.Words.Include(x=>x.CheckingHistories).Where(x=>x.BookId == bookId);
            courseContext.Words.RemoveRange(words);
            courseContext.Books.Remove(book);
            await courseContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
}
