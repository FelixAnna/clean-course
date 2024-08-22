using Entities.Entities;
using Services.Books.Models;

namespace Services.Books.Repositories;

public interface IBookRepository
{
    Task<IEnumerable<BookEntity>> GetAllAsync();
    Task<BookEntity?> GetByIdAsync(int bookId);
    Task<BookEntity> AddAsync(AddBookModel model);
    Task<BookEntity> UpdateAsync(int id, AddBookModel model);
    Task<bool> RemoveAsync(int bookId);
}
