namespace Services.BookCategories.Models
{
    public class SearchBookCategoryResult
    {
        public required IEnumerable<BookCategoryModel> Categories { get; set; }

        public required int Count { get; set; }
    }
}
