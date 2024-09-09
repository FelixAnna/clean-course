using Entities.Entities;

namespace Services.BookCategories;

public class BookCategoryModel(BookCategoryEntity entity)
{
    public int Id { get; set; } = entity.Id;

    public string CategoryName { get; set; } = entity.CategoryName;

    public bool Selected { get; set; } = entity.Selected;

    public string BookCategoryFullName => $"{CategoryName}";
}
