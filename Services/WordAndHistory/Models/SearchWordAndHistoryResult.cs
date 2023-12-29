namespace Services.WordAndHistory.Models;

public class SearchWordAndHistoryResult
{
    public IEnumerable<WordHistoryModel> Words { get; set; }

    public int Count { get; set; }
}
