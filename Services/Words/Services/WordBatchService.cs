using Entities.Entities;
using Services.CheckingHistories.Models;
using Services.Words.Models;
using Services.Words.Repositories;

namespace Services.Words;

public class WordBatchService(IWordRepository repository) : IWordBatchService
{
    private readonly IWordRepository repository = repository;
    public async Task<WordModel> AddWordAsync(AddWordModel model)
    {
        var (tobeInserted, tobeUpdated) = await GetWords(model, model);

        await repository.UpdateAllAsync([.. tobeUpdated]);
        var result = await repository.AddAsync([.. tobeInserted]);

        return  result != null ? new WordModel(result.First()) : new WordModel();
    }

    public async Task<IList<WordModel>> ImportWordsAsync(ImportWordsModel model)
    {
        var words = GetWords(model);

        var (tobeInserted, tobeUpdated) = await GetWords(model, [.. words]);

        await repository.UpdateAllAsync([.. tobeUpdated]);
        var results = await repository.AddAsync([.. tobeInserted]);

        return results.Select(x => new WordModel(x)).ToList();
    }

    public async Task<IList<WordModel>> PreviewWordsAsync(ImportWordsModel model)
    {
        var words = GetWords(model);

        var (tobeInserted, tobeUpdated) = await GetWords(model, [.. words]);

        return tobeInserted.Concat(tobeUpdated.Select(x => new AddWordModel()
        {
            Content = x.Content,
            BookId = x.BookId,
            Unit = x.Unit,
            Explanation = x.Explanation,
            Source = x.Source,
            Details = x.Details?? string.Empty,
        })).Select(x => new WordModel()
        {
            BookId = x.BookId,
            Content = x.Content,
            Explanation = x.Explanation,
            Source = x.Source,
            Details = x.Details,
            Unit = x.Unit,
            WordId = -1
        }).ToList();
    }

    private List<AddWordModel> GetWords(ImportWordsModel model)
    {
        if(model.Source == "pdf")
        {
            return model.Book.IsEnglish() ? EngWordFromPdfHelper.ParseWords(model) : WordFromPdfHelper.ParseWords(model);
        }
        else
        {
            return WordFromExcelHelper.ParseWords(model);
        }
    }

    private async Task<(IList<AddWordModel> tobeInserted, List<WordEntity> tobeDeleted)> GetWords(AddWordBaseModel addModel, params AddWordModel[] words)
    {
        var tobeInsertedNewWords = words.Where(x => Equals(x.BookId, addModel.BookId)).ToArray();
        var tobeUpdated = new List<WordEntity>();
        if (words != null && words.Length > 0)
        {
            var bookId = addModel.BookId;
            var isOverwrite = addModel.IsOverwrite;

            var existingWords = await repository.FindAsync(new SearchWordAndHistoryCriteria()
            {
                BookId = bookId,
            });

            if (isOverwrite)
            {
                foreach (var existingWord in existingWords)
                {
                    var newWord = tobeInsertedNewWords.Where(nw => existingWord.BookId == nw.BookId && existingWord.Content == nw.Content).FirstOrDefault();
                    if (newWord != null)
                    {
                        existingWord.Explanation = newWord.Explanation;
                        existingWord.Source = existingWord.Source ?? newWord.Source;
                        existingWord.Details = newWord.Details;
                        if (existingWord.Unit <= 0)
                        {
                            existingWord.Unit = newWord.Unit;
                        }

                        tobeUpdated.Add(existingWord);
                    }
                }

                tobeInsertedNewWords = tobeInsertedNewWords.Where(x => !tobeUpdated.Any(y => y.BookId == y.BookId && y.Content == x.Content)).ToArray();
            }
            else
            {
                tobeInsertedNewWords = tobeInsertedNewWords.Where(x => !existingWords.Any(y => y.BookId == y.BookId && y.Content == x.Content)).ToArray();
            }
        }

        return (tobeInsertedNewWords, tobeUpdated);
    }
}
