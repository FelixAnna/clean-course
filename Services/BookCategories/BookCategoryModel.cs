using Entities.Entities;

namespace Services.BookCategories;

public class BookCategoryModel(BookCategoryEntity entity)
{
    public int Id { get; set; } = entity.Id;

    public string CategoryName { get; set; } = entity.CategoryName;

    public string SharedCode { get; set; } = entity.SharedCode;

    public int Year { get; set; } = entity.Year;

    public string Grade { get; set; } = entity.Grade;

    public string Semester { get; set; } = entity.Semester;

    public bool Selected { get; set; } = entity.Selected;
}
