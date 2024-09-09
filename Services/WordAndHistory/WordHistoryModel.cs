using Entities.Entities;
using Services.Books;
using Shared.Models;
using System.Globalization;
using System.Linq;

namespace Services.WordAndHistory;

public class WordHistoryModel : BaseWordModel
{
    public BookModel Book { get; set; }
    public WordHistoryModel() { }
    public WordHistoryModel(WordEntity entity)
    {
        WordId = entity.WordId;
        BookId = entity.BookId;
        Content = entity.Content;
        Explanation = entity.Explanation;
        Details = entity.Details;
        Unit = entity.Unit;

        SetHistorySummary(entity.CheckingHistories);

        Book = new BookModel(entity.Book);
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


    public bool IsEnglish()
    {
        return IsMatch("英语" ,"English");
    }
    public bool IsChinese()
    {
        return IsMatch("语文", "Chinese");
    }
    public bool IsMath()
    {
        return IsMatch("数学", "Math");
    }

    private bool IsMatch(params string[] patterns)
    {
        foreach (var pattern in patterns)
        {
            if (Book.FriendlyName.Contains(pattern))
            {
                return true;
            }
        }

        return false;
    }
}
