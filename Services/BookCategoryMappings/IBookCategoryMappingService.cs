using Services.BookCategoryMappings.Models;

namespace Services.BookCategoryMappings;

public interface IBookCategoryMappingService
{
    Task AddAsync(int bookCategoryId, int bookId);
    Task<BookCategoryMappingsResult> GetByBookCategoryIdAsync(int bookCategoryId);
    Task<BookCategoryMappingsForAddModel> GetByBookCategoryIdForAddAsync(int bookCategoryId, string keywords);
    Task RemoveAsync(int bookCategoryId, int bookId);
}