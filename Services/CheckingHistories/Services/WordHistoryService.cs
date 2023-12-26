using Entities.Entities;
using Services.CheckingHistories;
using Services.CheckingHistories.Models;
using Services.Words;
using Services.Words.Models;

namespace Services.CheckingHistories.Services
{
    public class WordHistoryService(IWordRepository repository, ICheckingHistoryService checkingService) : IWordHistoryService
    {
        private readonly IWordRepository repository = repository;
        private readonly ICheckingHistoryService checkingService = checkingService;

        public async Task<string> ExportCheckingStatusAsync(ExportCheckingHistoryCriteria model)
        {
            var request = new SearchWordsCriteria()
            {
                KidId = model.KidId,
                SharedCode = model.SharedCode
            };

            var words = await repository.FindAsync(request);

            var index = 1;
            var results = words.Select(x =>
            {
                var wordInfo = $"{index++}\t{x.Content}\t{x.Explanation}\t{x.Unit}\t{x.Course}";
                var checkingInfo = x.CheckingHistories
                                    .OrderBy(y => y.CreatedTime)
                                    .Select(y => (!string.IsNullOrEmpty(y.Remark) ? y.Remark : y.IsCorrect ? 1 : 0) + "|" + y.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss"))
                                    .Aggregate(string.Empty, (accumulate, value) => $"{accumulate}\t{value}");
                var data = new
                {
                    x.SharedCode,
                    x.Course,
                    x.Unit,
                    Value = $"{wordInfo}\t{checkingInfo}"
                };

                return data;
            }).OrderBy(x => x.SharedCode).ThenBy(x => x.Course).ThenBy(x => x.Unit)
            .Aggregate(string.Empty, (accumulate, value) => $"{accumulate}{Environment.NewLine}{value.Value}");

            return results;
        }
        public async Task SaveCheckingStatusAsync(int wordId, int kidId, CheckingRemark status)
        {
            var model = new AddCheckingHistoryModel()
            {
                IsCorrect = status == CheckingRemark.Correct,
                WordId = wordId,
                KidId = kidId,
                Remark = status.ToString(),
            };
            await checkingService.AddAsync(model);
        }

        public async Task<IList<CheckingHistory>> ImportHistoryAsync(ImportCheckingHistoryModel model)
        {
            var toBeInserted = await GetWordWithHistoryAsync(model);

            if (toBeInserted.Count <= 0)
            {
                return new List<CheckingHistory>();
            }

            if (model.IsOverwrite)
            {
                await checkingService.RemoveAllByWordIdAsync(model.KidId, toBeInserted.Select(x => x.Id).ToArray());
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
            var results = await checkingService.AddAsync([.. historyList]);

            return results.ToList();
        }

        public async Task<IList<WordModel>> PreviewHistoryAsync(ImportCheckingHistoryModel model)
        {
            var toBeInserted = await GetWordWithHistoryAsync(model);

            if (toBeInserted.Count <= 0)
            {
                return new List<WordModel>();
            }

            return toBeInserted.Select(x => new WordModel()
            {
                Course = x.Course,
                Content = x.Content,
                Explanation = x.Explanation,
                Unit = x.Unit,
                SharedCode = model.SharedCode,
                WordId = x.Id,
                CheckingHistorySummary = $"{x.CheckingHistories.Count(x => x.Remark == CheckingRemark.Correct)}/ {x.CheckingHistories.Count}",
            }).ToList();
        }

        private static List<AddWordHistoryModel> ParseWords(ImportCheckingHistoryModel model)
        {
            var providedWords = new List<AddWordHistoryModel>();
            if (!string.IsNullOrEmpty(model.Content))
            {
                var wordsText = model.Content.Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
                foreach (var line in wordsText)
                {
                    //example: 230;肥料;féiliào;18;语文;Correct|2023-12-10 21:39:10;0|2023-12-10 21:39:11
                    var wordParts = line.Split(';', '；', '$', '\t').Select(x => x.Trim()).ToArray();
                    if (AddWordHistoryModelConvertor.IsValid(wordParts))
                    {
                        var addModel = AddWordHistoryModelConvertor.FromLine(model.SharedCode, model.Course, wordParts);
                        providedWords.Add(addModel);
                    }
                }
            }

            return providedWords;
        }
        private async Task<IList<AddWordHistoryModel>> GetWordWithHistoryAsync(ImportCheckingHistoryModel model)
        {
            var providedWords = ParseWords(model);
            var affectedExistingWords = new List<WordEntity>();

            var existingWords = await repository.FindAsync(new SearchWordsCriteria()
            {
                SharedCode = model.SharedCode,
                Course = model.Course,
            });


            providedWords = (from word in existingWords
                             from history in providedWords
                             where word.SharedCode == history.SharedCode && word.Course == history.Course && word.Content == history.Content && word.Unit == history.Unit
                             select new AddWordHistoryModel()
                             {
                                 Id = word.WordId,
                                 SharedCode = word.SharedCode,
                                 Course = word.Course,
                                 Content = word.Content,
                                 Unit = word.Unit,
                                 Explanation = word.Explanation,
                                 Overwrite = history.Overwrite,
                                 CheckingRecords = history.CheckingRecords,
                             }).ToList();

            return providedWords.Where(x => x.CheckingHistories != null && x.CheckingHistories.Any()).ToList();
        }
    }
}
