using Entities.Entities;
using Shared.Models;
using System.Globalization;

namespace Services.WordAndHistory;

public class WordHistoryModel : BaseWordModel
{
    public WordHistoryModel() { }
    public WordHistoryModel(WordEntity entity)
    {
        WordId = entity.WordId;
        SharedCode = entity.SharedCode;
        Course = entity.Course;
        Content = entity.Content;
        Explanation = entity.Explanation;
        Details = entity.Details;
        Unit = entity.Unit;

        SetHistorySummary(entity.CheckingHistories);
    }


    public string? CheckingHistorySummary { get; set; }

    public void SetHistorySummary(IEnumerable<CheckingHistoryEntity> histories)
    {
        var result = string.Empty;

        if (histories != null && histories.Any())
        {
            var success = histories.Count(x => x.IsCorrect);
            var failed = histories.Count(x => !x.IsCorrect);

            var percent = (success * 1.0 / (success + failed)).ToString("0%", CultureInfo.InvariantCulture);
            result = $"{percent}({success}/{success + failed})";
        }
        else
        {
            result = "--";
        }

        CheckingHistorySummary = result;
    }


}
