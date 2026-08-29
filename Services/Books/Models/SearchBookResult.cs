namespace Services.Books.Models;

public class SearchBookResult
{
    public IList<BookModel> Books { get; set; }

    public int Count { get; set; }
}
