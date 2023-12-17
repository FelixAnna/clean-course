using System.ComponentModel.DataAnnotations;

namespace Entities.Entities;

public class KidEntity
{
    [Key]
    public int KidId { get; set; }

    public required string Name { get; set; }

    public required int StartSchoolYear { get; set; }

    public bool Selected { get; set; }

    public IList<CheckingHistoryEntity> CheckingHistories { get; set; }
}
