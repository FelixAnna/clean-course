using Services.Books;

namespace Services.BookCategoryMappings.Models;

public class BookCategoryMappingsForAddModel : BookCategoryMappingsResult
{
    public List<BookModel> NewBooks { get; set; }
}
