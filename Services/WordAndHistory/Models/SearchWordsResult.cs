namespace Services.WordAndHistory.Models;

public class SearchWordAndHistoryResult
{
    public IEnumerable<WordModel> Words { get; set; }

    public int Count { get; set; }
}
