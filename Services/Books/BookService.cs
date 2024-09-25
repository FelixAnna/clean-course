using Services.Books.Models;
using Services.Books.Repositories;

namespace Services.Books;

public class BookService(IBookRepository repository) : IBookService
{
    private readonly IBookRepository repository = repository;

    public async Task<BookModel> AddAsync(AddBookModel model)
    {
        try
        {
            var result = await repository.AddAsync(model);
            return new BookModel(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
            throw;
        }
    }

    public async Task<bool> DeleteAsync(int bookId)
    {
        return await repository.RemoveAsync(bookId);
    }

    public async Task<SearchBookResult> GetAllAsync()
    {
        var categories = await repository.GetAllAsync();

        var results = categories.Select(x =>
        {
            return new BookModel(x);
        }).OrderByDescending(x => x.AuditYear).ThenBy(x => x.BookName).ToList();

        return new SearchBookResult()
        {
            Books = results,
            Count = results.Count
        };
    }

    public async Task<SearchBookResult> FindAsync(string keywords)
    {
        var categories = await repository.FindAsync(keywords);

        var results = categories.Select(x =>
        {
            return new BookModel(x);
        }).OrderByDescending(x => x.AuditYear).ThenBy(x => x.BookName).ToList();

        return new SearchBookResult()
        {
            Books = results,
            Count = results.Count
        };
    }

    public async Task<BookModel> GetByIdAsync(int bookId)
    {
        var result = await repository.GetByIdAsync(bookId);
        return new BookModel(result!);
    }

    public async Task<BookModel> UpdateAsync(int id, AddBookModel model)
    {
        var result = await repository.UpdateAsync(id, model);
        return new BookModel(result);
    }
}
