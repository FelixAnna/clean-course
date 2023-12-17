namespace Services.BookCategories.Models
{
    public class SearchBookCategoryResult
    {
        public IEnumerable<BookCategoryModel> Categories { get; set; }

        public int Count { get; set; }
    }
}
