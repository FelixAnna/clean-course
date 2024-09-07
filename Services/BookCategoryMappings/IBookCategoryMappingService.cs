using Services.BookCategoryMappings.Models;

namespace Services.BookCategoryMappings
{
    public interface IBookCategoryMappingService
    {
        Task AddBookCategoryMappingAsync(int bookCategoryId, int bookId);
        Task<BookCategoryMappingsResult> GetBookCategoryMappings(int bookCategoryId);
        Task RemoveBookCategoryMappingAsync(int bookCategoryId, int bookId);
    }
}