using System.ComponentModel.DataAnnotations;

namespace Services.Kids.Models
{
    public class AddKidModel
    {
        [Required]
        [StringLength(20, ErrorMessage = "Name is too long.")]
        public required string Name { get; set; }

        public required int StartSchoolYear { get; set; }
    }
}
