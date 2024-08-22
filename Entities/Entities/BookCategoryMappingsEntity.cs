namespace Entities.Entities
{
    public class BookCategoryMappingsEntity
    {
        public int BookCategoryId { get; set; }
        public int BookId { get; set; }

        public BookCategoryEntity BookCategory { get; set; }
        public BookEntity Book { get; set; }
    }
}
