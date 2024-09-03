using System.ComponentModel.DataAnnotations;

namespace Entities.Entities
{
    public class BookCategoryMappingsEntity
    {
        [Key]
        public int Id { get; set; }
        public int BookCategoryId { get; set; }
        public int BookId { get; set; }
        public BookCategoryEntity BookCategory { get; set; }
        public BookEntity Book { get; set; }
    }
}
