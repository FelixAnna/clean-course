namespace Services.BookCategoryWords.Models;

public class SearchWordsResult
{
    public IEnumerable<WordModel> Words { get; set; }

    public int Count { get; set; }
}
