namespace Services.Books.Models;

public class SearchBookResult
{
    public IEnumerable<BookModel> Books { get; set; }

    public int Count { get; set; }
}
