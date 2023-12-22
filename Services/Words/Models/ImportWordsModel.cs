using System.ComponentModel.DataAnnotations;

namespace Services.Words.Models;

public class ImportWordsModel
{
    [Required]
    public required string SharedCode { get; set; }

    [Required]
    public string Course { get; set; }

    [Required]
    public string Overwrite { get; set; }

    [Required]
    [StringLength(80000, ErrorMessage = "Content is too long.")]
    public string Content { get; set; }

    public bool IsOverwrite => Overwrite == "1";
}
