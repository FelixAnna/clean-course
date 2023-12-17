using System.ComponentModel.DataAnnotations;

namespace Services.Words.Models;

public class AddWordModel
{
    [Required]
    public required string SharedCode { get; set; }

    [Required]
    public string Course { get; set; }

    [Required]
    [StringLength(10, ErrorMessage = "Content is too long.")]
    public string Content { get; set; }

    [Required]
    [StringLength(10, ErrorMessage = "Explanation is too long.")]
    public string Explanation { get; set; }

    [Required]
    public int Unit { get; set; }
    [Required]
    public string Overwrite { get; set; }
    public bool IsOverwrite => Overwrite == "1";
}
