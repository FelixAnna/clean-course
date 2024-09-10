namespace Services.CheckingHistories.Models;

public class SearchWordAndHistoryResult
{
    public IEnumerable<CheckingHistoryModel> Words { get; set; }

    public int Count { get; set; }
}
