using System.ComponentModel.DataAnnotations;

namespace Services.BookCategoryWords.Models
{
    public abstract class AddWordBaseModel
    {
        [Required]
        public int BookId { get; set; }

        [Required]
        public string Overwrite { get; set; }
        public bool IsOverwrite => Overwrite == "1";
    }
}
