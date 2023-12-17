namespace Entities.Entities;

public class CheckingHistoryEntity
{
    public int Id { get; set; }

    public bool IsCorrect { get; set; }

    public string? Remark { get; set; }

    public DateTime CreatedTime { get; set; }

    public int WordId { get; set; }

    public int KidId { get; set; }

    public WordEntity Word { get; set; }
    public KidEntity Kid { get; set; }


}
