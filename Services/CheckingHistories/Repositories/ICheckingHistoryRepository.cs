using Entities.Entities;
using Services.CheckingHistories.Models;

namespace Services.CheckingHistories.Repositories;

public interface ICheckingHistoryRepository
{
    Task<IEnumerable<CheckingHistoryEntity>> GetByWordAndKidAsync(int wordId, int kidId);
    Task<CheckingHistoryEntity> AddAsync(AddCheckingHistoryModel model);
    Task<IEnumerable<CheckingHistoryEntity>> AddAsync(params AddCheckingHistoryModel[] models);
    Task<bool> RemoveAsync(int wordId);
    Task RemoveByWordIdsAsync(int kidId, params int[] wordIds);
}
