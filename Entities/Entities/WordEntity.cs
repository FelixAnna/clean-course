using System.ComponentModel.DataAnnotations;

namespace Entities.Entities;

public class WordEntity
{
    [Key]
    public int WordId { get; set; }

    public required string SharedCode { get; set; }

    public required string Course { get; set; }

    public required string Content { get; set; }

    public required string Explanation { get; set; }

    public int Unit { get; set; }

    public IList<CheckingHistoryEntity> CheckingHistories { get; set; }
}
