using Services.Books.Models;

namespace Services.Books
{
    public interface IBookService
    {
        Task<BookModel> AddAsync(AddBookModel model);
        Task<bool> DeleteAsync(int bookId);
        Task<SearchBookResult> GetAllAsync();
        Task<SearchBookResult> FindAsync(string keyword);
        Task<BookModel> UpdateAsync(int id, AddBookModel model);
        Task<BookModel> GetByIdAsync(int bookId);
    }
}
