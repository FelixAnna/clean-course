using Entities.Entities;
using Services.BookCategories.Models;

namespace Services.BookCategories.Repositories;

public interface IBookCategoryRepository
{
    Task<IEnumerable<BookCategoryEntity>> GetAllAsync();
    Task<BookCategoryEntity> AddAsync(AddBookCategoryModel model);
    Task<bool> RemoveAsync(int categoryId);
    Task<bool> SetDefaultAsync(int categoryId);
    Task<BookCategoryEntity?> GetDefaultAsync();
    Task<BookCategoryEntity?> GetByIdAsync(int categoryId);
    Task<BookCategoryEntity> UpdateAsync(int id, AddBookCategoryModel model);
}
