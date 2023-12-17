using Entities.Entities;
using Services.Kids.Models;

namespace Services.Kids.Repositories;

public interface IKidRepository
{
    Task<IEnumerable<KidEntity>> GetAllAsync();
    Task<KidEntity?> GetByIdAsync(int kidId);
    Task<KidEntity> AddAsync(AddKidModel model);
    Task<KidEntity> UpdateAsync(int id, AddKidModel model);
    Task<bool> RemoveAsync(int kidId);
    Task<bool> SetDefaultAsync(int kidId);
    Task<KidEntity?> GetDefaultAsync();
}
