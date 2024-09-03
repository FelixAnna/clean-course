using System.ComponentModel.DataAnnotations;

namespace Entities.Entities;

public class BookEntity
{
    [Key]
    public int BookId { get; set; }

    public string BookName { get; set; }

    public string Version { get; set; }

    public int AuditYear { get; set; }

    public string Grade { get; set; }

    public string Semester { get; set; }

    public IList<WordEntity> Words { get; set; }
}
