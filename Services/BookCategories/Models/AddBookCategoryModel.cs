using System.ComponentModel.DataAnnotations;

namespace Services.BookCategories.Models;

public class AddBookCategoryModel
{
    [Required]
    [StringLength(20, ErrorMessage = "CategoryName is too long.")]
    public required string CategoryName { get; set; }
}
