﻿using Entities.Entities;
using Services.CheckingHistories.Models;

namespace Services.CheckingHistories;

public class CheckingHistoryModel
{
    public CheckingHistoryModel(WordEntity wordEntity, params CheckingHistoryEntity[] history)
    {
        var word = new CheckingWord()
        {
            WordId = wordEntity.WordId,
            Content = wordEntity.Content,
            Course = wordEntity.Course,
            Details = wordEntity.Details,
            Explanation = wordEntity.Explanation,
        };

        var histories = history.Select(x =>
        {
            var model = new CheckingHistory()
            {
                CreatedTime = x.CreatedTime,
                Id = x.Id,
                IsCorrect = x.IsCorrect,
                Remark = x.Remark,
            };

            return model;
        }).ToList();

        Word = word;
        Histories = histories;

        Count = histories.Count;
    }
    public CheckingWord Word { get; set; }
    public IEnumerable<CheckingHistory> Histories { get; set; }
    public int Count { get; set; }

}
