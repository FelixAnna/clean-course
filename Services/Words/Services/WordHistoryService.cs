using Entities.Entities;
using Services.CheckingHistories;
using Services.CheckingHistories.Models;
using Services.Words.Models;

namespace Services.Words.Services
{
    public class WordHistoryService(IWordRepository repository, ICheckingHistoryService checkingService) : IWordHistoryService
    {
        private readonly IWordRepository repository = repository;
        private readonly ICheckingHistoryService checkingService = checkingService;

        public async Task<string> ExportCheckingStatusAsync(ExportWordHistoryModel model)
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
                                    .Select(y => (!string.IsNullOrEmpty(y.Remark) ? y.Remark : (y.IsCorrect?1:0)) + "|" + y.CreatedTime.ToString("yyyy-MM-dd HH:mm:ss"))
                                    .Aggregate(string.Empty, (accumulate, value) => $"{accumulate}\t{value}");
                var data = new
                {
                    x.SharedCode,
                    x.Course,
                    x.Unit,
                    Value = $"{wordInfo}\t{checkingInfo}"
                };

                return data;
            }).OrderBy(x => x.SharedCode).ThenBy(x=>x.Course).ThenBy(x => x.Unit)
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

        public async Task<IList<CheckingHistory>> ImportHistoryAsync(ImportWordHistoryModel model)
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

        public async Task<IList<WordModel>> PreviewHistoryAsync(ImportWordHistoryModel model)
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

        private static List<AddWordHistoryModel> ParseWords(ImportWordHistoryModel model)
        {
            var providedWords = new List<AddWordHistoryModel>();
            var wordsText = model.Content?.Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            for (int i = 0; i < wordsText?.Length; i++)
            {
                //230;肥料;féiliào;18;语文;Correct|2023-12-10 21:39:10;0|2023-12-10 21:39:11
                var wordParts = wordsText[i].Split(';', '；', '$', '\t').Select(x => x.Trim()).ToArray();
                if (wordParts.Length < 4 || !int.TryParse(wordParts[3], out int unit))
                {
                    continue;
                }

                var addModel = new AddWordHistoryModel()
                {
                    SharedCode = model.SharedCode,
                    Course = model.Course,
                    Content = wordParts[1],
                    Explanation = wordParts[2],
                    Unit = int.Parse(wordParts[3]),
                };

                if (wordParts.Length >= 5)
                {
                    addModel.Course = wordParts[4];
                }

                if (wordParts.Length > 5)
                {
                    addModel.CheckingRecords = wordParts.Skip(5).Where(x => !string.IsNullOrEmpty(x)).ToArray();
                }

                providedWords.Add(addModel);
            }

            return providedWords;
        }
        private async Task<IList<AddWordHistoryModel>> GetWordWithHistoryAsync(ImportWordHistoryModel model)
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
