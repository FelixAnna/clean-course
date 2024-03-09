using System.ComponentModel.DataAnnotations;

namespace Services.BookCategoryWords.Models;

public class ImportWordsModel : AddWordBaseModel
{

    [Required]
    [StringLength(80000, ErrorMessage = "Content is too long.")]
    public string Content { get; set; }
}
