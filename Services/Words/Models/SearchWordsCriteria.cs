using System.ComponentModel.DataAnnotations;

namespace Services.Words.Models;

public class SearchWordsCriteria
{
    public string Keyword { get; set; }

    public int BookCategoryId { get; set; }

    public int Unit { get; set; }

    public int BookId { get; set; }

    [Required]
    public int KidId { get; set; }

    public int PageIndex { get; set; }
}
