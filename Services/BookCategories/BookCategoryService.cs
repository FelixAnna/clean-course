using Services.BookCategories.Models;
using Services.BookCategories.Repositories;

namespace Services.BookCategories;

public class BookCategoryService(IBookCategoryRepository repository) : IBookCategoryService
{
    private readonly IBookCategoryRepository repository = repository;

    public async Task<SearchBookCategoryResult> GetAllAsync()
    {
        var categories = await repository.GetAllAsync();

        var results = categories.Select(x =>
        {
            return new BookCategoryModel(x);
        }).OrderByDescending(x => x.Selected).ToList();

        return new SearchBookCategoryResult()
        {
            Categories = results,
            Count = results.Count
        };
    }

    public async Task<BookCategoryModel> AddAsync(AddBookCategoryModel model)
    {
        var result = await repository.AddAsync(model);
        return new BookCategoryModel(result);
    }

    public async Task<BookCategoryModel> UpdateAsync(int id, AddBookCategoryModel model)
    {
        var result = await repository.UpdateAsync(id, model);
        return new BookCategoryModel(result);
    }

    public async Task<bool> DeleteAsync(int categoryId)
    {
        return await repository.RemoveAsync(categoryId);
    }

    public async Task<bool> SetDefaultAsync(int categoryId)
    {
        return await repository.SetDefaultAsync(categoryId);
    }
    public async Task<BookCategoryModel> GetDefaultAsync()
    {
        var result = await repository.GetDefaultAsync();
        return new BookCategoryModel(result ?? new Entities.Entities.BookCategoryEntity());
    }

    public async Task<BookCategoryModel> GetByIdAsync(int categoryId)
    {
        var result = await repository.GetByIdAsync(categoryId);
        return new BookCategoryModel(result!);
    }
}
