using Services.Kids;

namespace Services.CheckingHistories.Models;

public class CheckingHistory
{
    public int Id { get; set; }

    public bool IsCorrect { get; set; }

    public string? Remark { get; set; }

    public DateTime CreatedTime { get; set; }

    public KidModel Kid { get; set; }


    public string GetFormattedRemark()
    {
        if (!Enum.TryParse(Remark, out CheckingRemark eRemark))
        {
            return Remark ?? string.Empty;
        }

        switch (eRemark)
        {
            case CheckingRemark.Correct:
            case CheckingRemark.Incorrect:
                return string.Empty;
            case CheckingRemark.Translation:
                return "翻译错误";
            case CheckingRemark.Writing:
                return "拼写错误";
            case CheckingRemark.Pinyin:
                return "拼音错误";
            case CheckingRemark.Word:
                return "文字错误";
            default:
                break;
        }

        return string.Empty;
    }
}
