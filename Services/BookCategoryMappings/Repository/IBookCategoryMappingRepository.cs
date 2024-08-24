using Entities.Entities;

namespace Services.BookCategoryMappings.Repository
{
    public interface IBookCategoryMappingRepository
    {
        Task<IEnumerable<BookCategoryMappingsEntity?>> GetByBookCategoryIdAsync(int bookCategoryId);
        Task<BookCategoryMappingsEntity> AddAsync(int bookCategoryId, int bookId);
        Task<bool> RemoveAsync(int bookCategoryId, int bookId);
    }
}
