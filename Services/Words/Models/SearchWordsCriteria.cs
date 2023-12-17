using System.ComponentModel.DataAnnotations;

namespace Services.Words.Models;

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

    [Required]
    public int CheckingResult { get; set; }
}

public enum ECheckingResult
{
    None,
    Unchecked,
    Success,
    LastFailed,
    UsedFailed,
    InFrequent,
    RecentFailed
}
