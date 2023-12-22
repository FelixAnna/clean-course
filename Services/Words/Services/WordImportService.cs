using Entities.Entities;
using Services.CheckingHistories;
using Services.Words.Models;

namespace Services.Words.Services
{
    public class WordImportService(IWordRepository repository, ICheckingHistoryService checkingService) : IWordImportService
    {
        private readonly IWordRepository repository = repository;
        private readonly ICheckingHistoryService checkingService = checkingService;
        public async Task<WordModel> AddWordAsync(AddWordModel model)
        {
            var importModel = new ImportWordsModel
            {
                SharedCode = model.SharedCode,
                Course = model.Course,
                Overwrite = model.Overwrite,
                Content = $"{model.Content};{model.Explanation};{model.Unit}"
            };
            var (tobeInserted, tobeDeleted) = await GetWords(importModel);

            await repository.RemoveAllAsync([.. tobeDeleted]);
            var result = await repository.AddAsync([.. tobeInserted]);

            return new WordModel(result.First());
        }

        public async Task<IList<WordModel>> ImportWordsAsync(ImportWordsModel model)
        {
            var (tobeInserted, tobeDeleted) = await GetWords(model);

            await repository.RemoveAllAsync([.. tobeDeleted]);
            var results = await repository.AddAsync([.. tobeInserted]);

            return results.Select(x => new WordModel(x)).ToList();
        }

        public async Task<IList<WordModel>> PreviewWordsAsync(ImportWordsModel model)
        {
            var (tobeInserted, _) = await GetWords(model);

            return tobeInserted.Select(x => new WordModel()
            {
                Course = x.Course,
                Content = x.Content,
                Explanation = x.Explanation,
                Unit = x.Unit,
                SharedCode = model.SharedCode,
                WordId = -1
            }).ToList();
        }

        private static List<AddWordModel> ParseWords(ImportWordsModel model)
        {
            var tobeInsertedNewWords = new List<AddWordModel>();
            var wordsText = model.Content?.Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            for (int i = 0; i < wordsText?.Length; i++)
            {
                var wordParts = wordsText[i].Split(';', '；', '$', '\t').Select(x => x.Trim()).ToArray();

                if (AddWordModelConvertor.IsValid(wordParts))
                {
                    var addModel = AddWordModelConvertor.FromLine(model.SharedCode, model.Course, wordParts);
                    tobeInsertedNewWords.Add(addModel);
                }
            }

            return tobeInsertedNewWords;
        }
        private async Task<(IList<AddWordModel> tobeInserted, List<WordEntity> tobeDeleted)> GetWords(ImportWordsModel model)
        {
            var tobeInsertedNewWords = ParseWords(model);
            var tobeDeletedDuplicates = new List<WordEntity>();

            var existingWords = await repository.FindAsync(new SearchWordsCriteria()
            {
                SharedCode = model.SharedCode,
                Course = model.Course,
            });

            if (model.IsOverwrite)
            {
                tobeDeletedDuplicates = existingWords.Where(x => tobeInsertedNewWords.Any(y => y.SharedCode == x.SharedCode && y.Course == y.Course && y.Content == x.Content && y.Unit == x.Unit)).ToList();
            }
            else
            {
                tobeInsertedNewWords = tobeInsertedNewWords.Where(x => !existingWords.Any(y => y.SharedCode == x.SharedCode && y.Course == y.Course && y.Content == x.Content && y.Unit == x.Unit)).ToList();
            }

            return (tobeInsertedNewWords, tobeDeletedDuplicates);
        }
    }
}
