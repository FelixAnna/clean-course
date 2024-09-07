using Services.BookCategories;
using Services.BookCategoryMappings.Models;
using Services.BookCategoryMappings.Repository;
using Services.Books;

namespace Services.BookCategoryMappings;

public class BookCategoryMappingService : IBookCategoryMappingService
{
    private readonly IBookCategoryMappingRepository _repository;
    private readonly IBookCategoryService _categoryService;

    public BookCategoryMappingService(IBookCategoryMappingRepository repository, IBookCategoryService categoryService)
    {
        _repository = repository;
        _categoryService = categoryService;
    }

    public async Task<BookCategoryMappingsResult> GetBookCategoryMappings(int bookCategoryId)
    {
        var mappings = await _repository.GetByBookCategoryIdAsync(bookCategoryId);

        var result = new BookCategoryMappingsResult();
        if (!mappings.Any())
        {
            result.BookCategory = await _categoryService.GetByIdAsync(bookCategoryId);
            return result;
        }

        result.BookCategory = new BookCategoryModel(mappings.First()!.BookCategory);
        result.Books = mappings.Select(x => new BookModel(x.Book)).ToList();

        return result;
    }

    public async Task AddBookCategoryMappingAsync(int bookCategoryId, int bookId)
    {
        await _repository.AddAsync(bookCategoryId, bookId);
    }

    public async Task RemoveBookCategoryMappingAsync(int bookCategoryId, int bookId)
    {
        await _repository.RemoveAsync(bookCategoryId, bookId);
    }
}
