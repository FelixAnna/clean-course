using Services.CheckingHistories.Models;
using Services.CheckingHistories.Repositories;
using Services.Words.Repositories;

namespace Services.CheckingHistories;

public class CheckingHistoryService(ICheckingHistoryRepository repository, IWordRepository wordRepository) : ICheckingHistoryService
{
    private readonly ICheckingHistoryRepository repository = repository;
    private readonly IWordRepository wordRepository = wordRepository;

    public async Task<SearchWordAndHistoryResult> GetWordsAsync(SearchWordAndHistoryCriteria request)
    {
        var words = await wordRepository.FindAsync(request);

        var results = words.Select(x =>
        {
            return new CheckingHistoryModel(x, x.CheckingHistories);
        }).OrderBy(x => x.BookId).ThenBy(x => x.Unit).ToList();

        return new SearchWordAndHistoryResult()
        {
            Words = results,
            Count = results.Count
        };
    }

    public async Task<CheckingHistoryModel> GetByWordAndKidAsync(int wordId, int kidId)
    {
        var word = await wordRepository.GetByIdAsync(wordId);
        var history = await repository.GetByWordAndKidAsync(wordId, kidId);

        return new CheckingHistoryModel(word!, history.ToArray());
    }

    public async Task<CheckingHistoryModel> AddAsync(AddCheckingHistoryModel model)
    {
        var word = await wordRepository.GetByIdAsync(model.WordId);
        var result = await repository.AddAsync(model);
        return new CheckingHistoryModel(word!, []);
    }

    public async Task<bool> DeleteAsync(int historyId)
    {
        return await repository.RemoveAsync(historyId);
    }
}
