using System.ComponentModel.DataAnnotations;

namespace Entities.Entities;

public class WordEntity
{
    [Key]
    public int WordId { get; set; }

    public required int BookId { get; set; }

    public required string Content { get; set; }

    public required string Explanation { get; set; }

    public string? Details { get; set; }

    public int Unit { get; set; }
    
    public string Source { get; set; }

    public IList<CheckingHistoryEntity> CheckingHistories { get; set; }

    public BookEntity Book { get; set; }
}
