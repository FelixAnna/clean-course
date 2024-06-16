using System.ComponentModel.DataAnnotations;

namespace Services.BookCategoryWords.Models;

public class SearchWordsCriteria
{
    public string Course { get; set; }

    [StringLength(10, ErrorMessage = "Content is too long.")]
    public string Content { get; set; }

    [StringLength(10, ErrorMessage = "Explanation is too long.")]
    public string Explanation { get; set; }

    public int Unit { get; set; }

    [Required]
    public string SharedCode { get; set; }

    [Required]
    public int KidId { get; set; }

    public int PageIndex { get; set; }
}
