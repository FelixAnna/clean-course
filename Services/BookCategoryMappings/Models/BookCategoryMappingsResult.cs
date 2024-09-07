using Services.BookCategories;
using Services.Books;

namespace Services.BookCategoryMappings.Models
{
    public class BookCategoryMappingsResult
    {
        public List<BookModel> Books { get; set; }

        public BookCategoryModel BookCategory { get; set; }
    }
}
