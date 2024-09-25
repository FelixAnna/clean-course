using Entities.Entities;
using Services.Words.Models;

namespace Services.Words.Repositories;

public interface IWordRepository
{
    Task<WordEntity?> GetByIdAsync(int id);
    Task<IList<WordEntity>> FindAsync(SearchWordsCriteria request);
    Task<IEnumerable<WordEntity>> AddAsync(params AddWordModel[] models);
    Task<WordEntity> UpdateAsync(int id, AddWordModel model);
    Task<bool> RemoveAsync(int wordId);
    Task UpdateAllAsync(params WordEntity[] entities);
}