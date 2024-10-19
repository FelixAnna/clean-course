namespace Services.CheckingHistories.Models;

public class AddCheckingHistoryModel
{
    public int WordId { get; set; }
    
    public int KidId { get; set; }
    
    public bool IsCorrect { get; set; }

    public string? Remark { get; set; }

    public DateTime? CreatedTime { get; set; }
}

public enum CheckingRemark
{
    Correct = 1,
    Incorrect,

    Writing,
    Translation,

    Word,
    Pinyin,


    WrittingCorrect,
    WrittingIncorrect,
}
