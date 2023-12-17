using System.ComponentModel.DataAnnotations;

namespace Services.BookCategories.Models;

public class AddBookCategoryModel
{
    [Required]
    [StringLength(20, ErrorMessage = "CategoryName is too long.")]
    public required string CategoryName { get; set; }

    [Required]
    public int Year { get; set; }

    [Required]
    public required string Grade { get; set; }

    [Required]
    public required string Semester { get; set; }
}
