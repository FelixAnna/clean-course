using Entities.Entities;
using Services.BookCategoryWords.Models;
using Services.WordAndHistory.Models;
using Services.WordAndHistory.Repositories;

namespace Services.BookCategoryWords
{
    public class WordBatchService(IWordRepository repository) : IWordBatchService
    {
        private readonly IWordRepository repository = repository;
        public async Task<WordModel> AddWordAsync(AddWordModel model)
        {
            var importModel = new ImportWordsModel
            {
                SharedCode = model.SharedCode,
                Course = model.Course,
                Overwrite = model.Overwrite,
                Content = $"1;{model.Content};{model.Explanation};{model.Unit};{model.Course}"
            };
            var (tobeInserted, tobeUpdated) = await GetWords(importModel);

            await repository.UpdateAllAsync([.. tobeUpdated]);
            var result = await repository.AddAsync([.. tobeInserted]);

            return new WordModel(result.First());
        }

        public async Task<IList<WordModel>> ImportWordsAsync(ImportWordsModel model)
        {
            var (tobeInserted, tobeUpdated) = await GetWords(model);

            await repository.UpdateAllAsync([.. tobeUpdated]);
            var results = await repository.AddAsync([.. tobeInserted]);

            return results.Select(x => new WordModel(x)).ToList();
        }

        public async Task<IList<WordModel>> PreviewWordsAsync(ImportWordsModel model)
        {
            var (tobeInserted, tobeUpdated) = await GetWords(model);

            return tobeInserted.Concat(tobeUpdated.Select(x => new AddWordModel()
            {
                Content = x.Content,
                SharedCode = x.SharedCode,
                Course = x.Course,
                Unit = x.Unit,
                Explanation = x.Explanation,
            })).Select(x => new WordModel()
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
            var tobeUpdated = new List<WordEntity>();

            var existingWords = await repository.FindAsync(new SearchWordAndHistoryCriteria()
            {
                SharedCode = model.SharedCode,
                Course = model.Course,
            });

            if (model.IsOverwrite)
            {
                foreach (var w in existingWords)
                {
                    var nw = tobeInsertedNewWords.Where(nw => w.SharedCode == nw.SharedCode && w.Course == nw.Course && w.Content == nw.Content && w.Unit == nw.Unit).FirstOrDefault();
                    if (nw != null)
                    {
                        w.Explanation = nw.Explanation;
                        tobeUpdated.Add(w);
                    }
                }

                tobeInsertedNewWords = tobeInsertedNewWords.Where(x => !tobeUpdated.Any(y => y.SharedCode == x.SharedCode && y.Course == y.Course && y.Content == x.Content && y.Unit == x.Unit)).ToList();
            }
            else
            {
                tobeInsertedNewWords = tobeInsertedNewWords.Where(x => !existingWords.Any(y => y.SharedCode == x.SharedCode && y.Course == y.Course && y.Content == x.Content && y.Unit == x.Unit)).ToList();
            }

            return (tobeInsertedNewWords, tobeUpdated);
        }
    }
}
