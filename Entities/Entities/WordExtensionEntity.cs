using System.ComponentModel.DataAnnotations;

namespace Entities.Entities;

public class WordExtensionEntity
{
    [Key]
    public int Id { get; set; }

    public int WordId { get; set; }

    public required string Name { get; set; }

    public required string Content { get; set; }

    public WordEntity Word { get; set; }
}