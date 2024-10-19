using Services.BookCategories;
using Services.BookCategoryMappings.Models;
using Services.BookCategoryMappings.Repository;
using Services.Books;

namespace Services.BookCategoryMappings;

public class BookCategoryMappingService(IBookCategoryMappingRepository repository, IBookCategoryService categoryService, IBookService bookService) : IBookCategoryMappingService
{
    private readonly IBookCategoryMappingRepository _repository = repository;
    private readonly IBookCategoryService _categoryService = categoryService;
    private readonly IBookService _bookService = bookService;

    public async Task<BookCategoryMappingsForAddModel> GetByBookCategoryIdForAddAsync(int bookCategoryId, string keywords)
    {
        var mappings = await _repository.GetByBookCategoryIdAsync(bookCategoryId);
        var relatedBooks = await _bookService.FindAsync(keywords);

        var result = new BookCategoryMappingsForAddModel();
        if (!mappings.Any())
        {
            result.BookCategory = await _categoryService.GetByIdAsync(bookCategoryId);
            result.NewBooks = relatedBooks.Books.ToList();
            return result;
        }

        result.BookCategory = new BookCategoryModel(mappings.First()!.BookCategory);
        result.LinkedBooks = mappings.Select(x => new BookModel(x!.Book)).ToList();
        result.NewBooks = relatedBooks.Books.Where(x=> !result.LinkedBooks.Any(y=>x.BookId == y.BookId)).ToList();
        return result;
    }

    public async Task AddAsync(int bookCategoryId, int bookId)
    {
        await _repository.AddAsync(bookCategoryId, bookId);
    }

    public async Task RemoveAsync(int bookCategoryId, int bookId)
    {
        await _repository.RemoveAsync(bookCategoryId, bookId);
    }

    public async Task<BookCategoryMappingsResult> GetByBookCategoryIdAsync(int bookCategoryId)
    {
        var mappings = await _repository.GetByBookCategoryIdAsync(bookCategoryId);

        var result = new BookCategoryMappingsResult();
        if (!mappings.Any())
        {
            result.BookCategory = await _categoryService.GetByIdAsync(bookCategoryId);
            result.LinkedBooks = [];
            return result;
        }

        result.BookCategory = new BookCategoryModel(mappings.First()!.BookCategory);
        result.LinkedBooks = mappings.Select(x => new BookModel(x!.Book)).ToList();
        return result;
    }
}
