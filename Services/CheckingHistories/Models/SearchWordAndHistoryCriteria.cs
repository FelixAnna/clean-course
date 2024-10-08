﻿using Services.Words.Models;
using System.ComponentModel.DataAnnotations;

namespace Services.CheckingHistories.Models;

public class SearchWordAndHistoryCriteria : SearchWordsCriteria
{
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
