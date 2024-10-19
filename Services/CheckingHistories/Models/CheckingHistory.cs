using Entities.Entities;
using Services.Kids;

namespace Services.CheckingHistories.Models;

public class CheckingHistory(CheckingHistoryEntity historyEntity)
{
    public int Id { get; set; } = historyEntity.Id;

    public bool IsCorrect { get; set; } = historyEntity.IsCorrect;

    public string? Remark { get; set; } = historyEntity.Remark;

    public DateTime CreatedTime { get; set; } = historyEntity.CreatedTime;

    public KidModel Kid { get; set; } = new KidModel(historyEntity.Kid);


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
            case CheckingRemark.WrittingCorrect:
                return "默写正确";
            case CheckingRemark.WrittingIncorrect:
                return "默写错误";
            default:
                break;
        }

        return string.Empty;
    }
}
