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
            var tobeInserted = await GetWordWithHistoryAsync(model);

            if (tobeInserted.Count <= 0)
            {
                return new List<CheckingHistory>();
            }

            if (model.IsOverwrite)
            {
                await checkingService.RemoveAllByWordIdAsync(model.KidId, tobeInserted.Select(x => x.Id).ToArray());
            }

            var historyList = tobeInserted.SelectMany(x =>
            {
                var checkingList = new List<AddCheckingHistoryModel>();

                for (var i = 0; i < x.CheckingHistories.Count(); i++)
                {
                    var remark = CheckingRemark.Incorrect;
                    if (Enum.TryParse(x.CheckingHistories[i], out CheckingRemark status))
                    {
                        remark = status;
                    }
                    else if (int.TryParse(x.CheckingHistories[i], out int intStatus))
                    {
                        try
                        {
                            remark = (CheckingRemark)intStatus;
                        }
                        catch
                        {
                            remark = CheckingRemark.Incorrect;
                        }
                    }
                    else
                    {
                        remark = CheckingRemark.Incorrect;
                    }

                    checkingList.Add(new AddCheckingHistoryModel()
                    {
                        IsCorrect = x.History[i],
                        KidId = model.KidId,
                        WordId = x.Id,
                        Remark = x.History[i] ? CheckingRemark.Correct.ToString() : status.ToString(),
                    });
                }
                return checkingList;
            }).ToList();
            var results = await checkingService.AddAsync([.. historyList]);

            return results.ToList();
        }

        public async Task<IList<WordModel>> PreviewHistoryAsync(ImportWordHistoryModel model)
        {
            var tobeInserted = await GetWordWithHistoryAsync(model);

            if (tobeInserted.Count <= 0)
            {
                return new List<WordModel>();
            }

            return tobeInserted.Select(x => new WordModel()
            {
                Course = x.Course,
                Content = x.Content,
                Explanation = x.Explanation,
                Unit = x.Unit,
                SharedCode = model.SharedCode,
                WordId = x.Id,
                CheckingHistorySummary = $"{x.History.Count(x => x == true)}/ {x.History.Count()}",
            }).ToList();
        }

        private static List<AddWordHistoryModel> ParseWords(ImportWordHistoryModel model)
        {
            var providedWords = new List<AddWordHistoryModel>();
            var wordsText = model.Content?.Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            for (int i = 0; i < wordsText?.Length; i++)
            {
                var wordParts = wordsText[i].Split(';', '；', '$', '\t').Select(x => x.Trim()).ToArray();
                if (wordParts.Length < 3 || !int.TryParse(wordParts[2], out int unit))
                {
                    continue;
                }

                var addModel = new AddWordHistoryModel()
                {
                    SharedCode = model.SharedCode,
                    Course = model.Course,
                    Content = wordParts[0],
                    Explanation = wordParts[1],
                    Unit = int.Parse(wordParts[2]),
                };

                if (wordParts.Length == 4)
                {
                    addModel.Course = wordParts[3];
                }

                if (wordParts.Length > 4)
                {
                    addModel.CheckingHistories = wordParts.Skip(4).Where(x => !string.IsNullOrEmpty(x)).ToArray();
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
                                 CheckingHistories = history.CheckingHistories,
                             }).ToList();

            return providedWords.Where(x => x.History != null && x.History.Any()).ToList();
        }
    }
}
