using System.ComponentModel.DataAnnotations;

namespace Services.Words.Models;

public class ImportWordsModel : AddWordBaseModel
{

    [Required]
    [StringLength(80000, ErrorMessage = "Content is too long.")]
    public string Content { get; set; }

    public string Source { get; set; }
}
