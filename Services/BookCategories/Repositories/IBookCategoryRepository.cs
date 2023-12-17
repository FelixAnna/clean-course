using Entities.Entities;
using Services.BookCategories.Models;

namespace Services.BookCategories.Repositories;

public interface IBookCategoryRepository
{
    Task<IEnumerable<BookCategoryEntity>> GetAllAsync();
    Task<BookCategoryEntity> AddAsync(AddBookCategoryModel model);
    Task<bool> RemoveAsync(int categorId);
    Task<bool> SetDefaultAsync(int categorId);
    Task<BookCategoryEntity?> GetDefaultAsync();
    Task<BookCategoryEntity?> GetByIdAsync(int categorId);
    Task<BookCategoryEntity> UpdateAsync(int id, AddBookCategoryModel model);
}
