using System.ComponentModel.DataAnnotations;

namespace Services.BookCategoryWords.Models
{
    public abstract class AddWordBaseModel
    {
        [Required]
        public required string SharedCode { get; set; }

        [Required]
        public string Course { get; set; }

        [Required]
        public string Overwrite { get; set; }
        public bool IsOverwrite => Overwrite == "1";
    }
}
