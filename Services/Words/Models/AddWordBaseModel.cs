using System.ComponentModel.DataAnnotations;

namespace Services.Words.Models
{
    public abstract class AddWordBaseModel
    {
        [Required]
        public int BookId { get; set; }

        public string BookName { get; set; }

        [Required]
        public string Overwrite { get; set; }
        public bool IsOverwrite => Overwrite == "1";
    }
}
