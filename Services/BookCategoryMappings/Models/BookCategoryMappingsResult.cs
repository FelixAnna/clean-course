using Services.BookCategories;
using Services.Books;

namespace Services.BookCategoryMappings.Models
{
    public class BookCategoryMappingsResult
    {
        public List<BookModel> LinkedBooks { get; set; }

        public BookCategoryModel BookCategory { get; set; }
    }
}
