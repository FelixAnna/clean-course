using System.ComponentModel.DataAnnotations;

namespace Services.Books.Models
{
    public class AddBookModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "Name is too long.")]

        public required string BookName { get; set; }

        public string Version { get; set; }

        public int AuditYear { get; set; }

        public string Grade { get; set; }

        public string Semester { get; set; }
    }
}
