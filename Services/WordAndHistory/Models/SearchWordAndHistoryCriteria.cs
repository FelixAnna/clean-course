using Services.BookCategoryWords.Models;
using System.ComponentModel.DataAnnotations;

namespace Services.WordAndHistory.Models;

public class SearchWordAndHistoryCriteria : SearchWordsCriteria
{
    [Required]
    public int CheckingResult { get; set; }

    public int PageIndex {  get; set; }
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
