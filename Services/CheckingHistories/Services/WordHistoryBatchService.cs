using Entities.Entities;
using Services.Books;
using Services.CheckingHistories.Models;
using Services.CheckingHistories.Models.Admin;
using Services.CheckingHistories.Repositories;
using Services.Words.Repositories;

namespace Services.CheckingHistories.Services;

public class WordHistoryBatchService(ICheckingHistoryRepository repository, IWordRepository wordRepository) : IWordHistoryBatchService
{
    private const string splitter = "\t";
    private readonly ICheckingHistoryRepository repository = repository;
    private readonly IWordRepository wordRepository = wordRepository;

    public async Task<string> ExportCheckingStatusAsync(ExportCheckingHistoryCriteria model)
    {
        var request = new SearchWordAndHistoryCriteria()
        {
            KidId = model.KidId,
            BookCategoryId = model.BookCategoryId,
            BookId =  model.BookId??0,
        };

        var words = await wordRepository.FindAsync(request);

        var index = 1;
        var results = words.Select(x =>
        {
            var wordInfo = $"{index++}{splitter}{Encode(x.Content)}{splitter}{Encode(x.Explanation)}{splitter}{Encode(x.Details)}{splitter}{x.Unit}{splitter}{x.BookId}";
            var checkingInfo = x.CheckingHistories
                                .OrderBy(y => y.CreatedTime)
                                .Select(y => (!string.IsNullOrEmpty(y.Remark) ? y.Remark : y.IsCorrect ? 1 : 0) + "|" + y.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss"))
                                .Aggregate(string.Empty, (accumulate, value) => $"{accumulate}{splitter}{value}");
            var data = new
            {
                x.BookId,
                x.Unit,
                Value = $"{wordInfo}{splitter}{checkingInfo}"
            };

            return data;
        }).OrderBy(x => x.BookId).ThenBy(x => x.Unit)
        .Aggregate(string.Empty, (accumulate, value) => $"{accumulate}{Environment.NewLine}{value.Value}");

        return results;
    }

    private static string Encode(string value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;

        var encodedValue = value.Replace("\n", "\\n").Trim();
        return encodedValue;
    }

    public async Task<int> ImportHistoryAsync(ImportCheckingHistoryModel model)
    {
        var toBeInserted = await GetWordWithHistoryAsync(model);

        if (toBeInserted.Count <= 0)
        {
            return 0;
        }

        if (model.IsOverwrite)
        {
            await repository.RemoveByWordIdsAsync(model.KidId, toBeInserted.Select(x => x.Id).ToArray());
        }

        var historyList = toBeInserted.SelectMany(x =>
        {
            var checkingList = new List<AddCheckingHistoryModel>();

            for (var i = 0; i < x.CheckingRecords?.Count; i++)
            {
                checkingList.Add(new AddCheckingHistoryModel()
                {
                    IsCorrect = x.CheckingHistories[i].Remark == CheckingRemark.Correct,
                    KidId = model.KidId,
                    WordId = x.Id,
                    Remark = x.CheckingHistories[i].Remark.ToString(),
                    CreatedTime = x.CheckingHistories[i].CreatedTime
                });
            }
            return checkingList;
        }).ToList();
        var results = await repository.AddAsync([.. historyList]);

        return results.Count();
    }

    public async Task<IList<CheckingHistoryModel>> PreviewHistoryAsync(ImportCheckingHistoryModel model)
    {
        var toBeInserted = await GetWordWithHistoryAsync(model);

        if (toBeInserted.Count <= 0)
        {
            return new List<CheckingHistoryModel>();
        }

        return toBeInserted.Select(x => new CheckingHistoryModel()
        {
            BookId = model.BookId??0,
            Book = x.Book,
            Content = x.Content,
            Explanation = x.Explanation,
            Details = x.Details,
            Unit = x.Unit,
            WordId = x.Id,
            CheckingHistorySummary = $"{x.CheckingHistories.Count(x => x.Remark == CheckingRemark.Correct)}/ {x.CheckingHistories.Count}",
        }).ToList();
    }

    private static List<WordCheckingHistoriesModel> ParseWords(ImportCheckingHistoryModel model)
    {
        var providedWords = new List<WordCheckingHistoriesModel>();
        if (!string.IsNullOrEmpty(model.Content))
        {
            var wordsText = model.Content.Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            foreach (var line in wordsText)
            {
                //example: 230;肥料;féiliào;18;语文;Correct|2023-12-10 21:39:10;0|2023-12-10 21:39:11
                var wordParts = line.Split(splitter).Select(x => x.Trim()).ToArray();
                if (AddWordHistoryModelConvertor.IsValid(wordParts))
                {
                    var addModel = AddWordHistoryModelConvertor.FromLine(model.BookId??0, wordParts);
                    providedWords.Add(addModel);
                }
            }
        }

        return providedWords;
    }
    private async Task<IList<WordCheckingHistoriesModel>> GetWordWithHistoryAsync(ImportCheckingHistoryModel model)
    {
        var providedWords = ParseWords(model);
        var affectedExistingWords = new List<WordEntity>();

        var existingWords = await wordRepository.FindAsync(new SearchWordAndHistoryCriteria()
        {
            KidId = model.KidId,
            BookId = model.BookId??0,
            BookCategoryId = model.BookCategoryId,
        });


        providedWords = (from word in existingWords
                         from history in providedWords
                         where word.BookId == history.BookId && word.Content == history.Content && (word.Unit == history.Unit || history.Unit <= 0)
                         select new WordCheckingHistoriesModel()
                         {
                             Id = word.WordId,
                             Book = new BookModel(word.Book),
                             BookId = word.BookId,
                             Content = word.Content,
                             Unit = word.Unit,
                             Explanation = word.Explanation,
                             Details = word.Details,
                             Overwrite = history.Overwrite,
                             CheckingRecords = history.CheckingRecords,
                         }).ToList();

        return providedWords.Where(x => x.CheckingHistories != null && x.CheckingHistories.Any()).ToList();
    }
}
