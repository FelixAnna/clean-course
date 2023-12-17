﻿using Entities.Entities;
using Shared.Models;
using System.Globalization;

namespace Services.Words;

public class WordModel : BaseWordModel
{
    public WordModel() { }
    public WordModel(WordEntity entity)
    {
        WordId = entity.WordId;
        SharedCode = entity.SharedCode;
        Course = entity.Course;
        Content = entity.Content;
        Explanation = entity.Explanation;
        Unit = entity.Unit;

        SetHistorySummary(entity.CheckingHistories);
    }

    public string SharedCode { get; set; }

    public int Unit { get; set; }
    public string? CheckingHistorySummary { get; set; }

    public void SetHistorySummary(IEnumerable<CheckingHistoryEntity> histories)
    {
        var result = string.Empty;

        if (histories != null && histories.Any())
        {
            var success = histories.Count(x => x.IsCorrect);
            var failed = histories.Count(x => !x.IsCorrect);

            var percent = (success * 1.0 / (success + failed)).ToString("P", CultureInfo.InvariantCulture);
            result = $"{percent}({success}/{success + failed})";
        }
        else
        {
            result = "--";
        }

        CheckingHistorySummary = result;
    }


}
