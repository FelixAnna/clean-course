using Services.BookCategoryMappings.Models;

namespace Services.BookCategoryMappings
{
    public interface IBookCategoryMappingService
    {
        Task AddBookCategoryMappingAsync(int bookCategoryId, int bookId);
        Task<BookCategoryMappingsResult> GetBookCategoryMappingsById(int bookCategoryId);
        Task<BookCategoryMappingsForAddModel> GetBookCategoryMappingsByForAdd(int bookCategoryId, string keywords);
        Task RemoveBookCategoryMappingAsync(int bookCategoryId, int bookId);
    }
}