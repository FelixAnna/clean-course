using Services.BookCategories.Models;

namespace Services.BookCategories
{
    public interface IBookCategoryService
    {
        Task<BookCategoryModel> AddAsync(AddBookCategoryModel model);
        Task<bool> DeleteAsync(int categoryId);
        Task<SearchBookCategoryResult> GetAllAsync();
        Task<bool> SetDefaultAsync(int categoryId);
        Task<BookCategoryModel> GetDefaultAsync();
        Task<BookCategoryModel> UpdateAsync(int id, AddBookCategoryModel model);
        Task<BookCategoryModel> GetByIdAsync(int categoryId);
    }
}