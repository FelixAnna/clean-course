﻿using Services.CheckingHistories.Models;
using Services.CheckingHistories.Repositories;
using Services.WordAndHistory.Repositories;

namespace Services.CheckingHistories;

public class CheckingHistoryService(ICheckingHistoryRepository repository, IWordRepository wordRepository) : ICheckingHistoryService
{
    private readonly ICheckingHistoryRepository repository = repository;
    private readonly IWordRepository wordRepository = wordRepository;

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
        return new CheckingHistoryModel(word!, result);
    }

    public async Task<bool> DeleteAsync(int historyId)
    {
        return await repository.RemoveAsync(historyId);
    }
}
