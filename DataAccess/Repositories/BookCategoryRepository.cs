using Entities;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Services.BookCategories.Models;
using Services.BookCategories.Repositories;

namespace DataAccess.Repositories;

public class BookCategoryRepository(AbstractCourseContext courseContext) : IBookCategoryRepository
{
    private readonly AbstractCourseContext courseContext = courseContext;
    public async Task<BookCategoryEntity> AddAsync(AddBookCategoryModel model)
    {
        string code = GenerateCode(model);
        var result = courseContext.BookCategories.Add(new BookCategoryEntity
        {
            SharedCode = code,
            CategoryName = model.CategoryName,
            Grade = model.Grade,
            Semester = model.Semester,
            Year = model.Year
        });
        await courseContext.SaveChangesAsync();
        return result.Entity;
    }

    private static string GenerateCode(AddBookCategoryModel model)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes($"{model.CategoryName}-{model.Year}-{model.Grade}-{model.Semester}");
        var code = Convert.ToBase64String(plainTextBytes);
        return code;
    }

    public async Task<BookCategoryEntity> UpdateAsync(int id, AddBookCategoryModel model)
    {
        var category = await courseContext.BookCategories.FirstAsync(x => x.Id == id);
        category.CategoryName = model.CategoryName;
        category.Grade = model.Grade;
        category.Semester = model.Semester;
        category.Year = model.Year;
        await courseContext.SaveChangesAsync();
        return category;
    }

    public async Task<IEnumerable<BookCategoryEntity>> GetAllAsync()
    {
        return await courseContext.BookCategories.ToListAsync();
    }

    public async Task<BookCategoryEntity?> GetByIdAsync(int categoryId)
    {
        var category = await courseContext.BookCategories.FindAsync(categoryId);
        return category;
    }

    public async Task<bool> RemoveAsync(int categoryId)
    {
        var category = await courseContext.BookCategories.FindAsync(categoryId);
        if (category != null)
        {
            var relatedWords = courseContext.Words.Include(x => x.CheckingHistories)
                .Where(w => w.SharedCode == category.SharedCode)
                .ToList();
            courseContext.Words.RemoveRange(relatedWords);
            await courseContext.SaveChangesAsync();

            courseContext.BookCategories.Remove(category);
            await courseContext.SaveChangesAsync();
            return true;
        }

        return false;
    }
    public async Task<bool> SetDefaultAsync(int categoryId)
    {
        var allCategories = await courseContext.BookCategories.ToArrayAsync();
        foreach (var category in allCategories)
        {
            category.Selected = category.Id == categoryId;
        }

        courseContext.BookCategories.UpdateRange(allCategories);
        await courseContext.SaveChangesAsync();
        return true;
    }

    public async Task<BookCategoryEntity?> GetDefaultAsync()
    {
        var category = await courseContext.BookCategories.FirstOrDefaultAsync(x => x.Selected);
        return category;
    }
}
