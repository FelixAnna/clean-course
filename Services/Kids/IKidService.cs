using Services.Kids.Models;

namespace Services.Kids;

public interface IKidService
{
    Task<KidModel> AddKidAsync(AddKidModel model);
    Task<bool> UnRegisterKidAsync(int kidId);
    Task<SearchKidResult> GetAllAsync();
    Task<bool> SetDefaultAsync(int kidId);
    Task<KidModel> GetDefaultAsync();
    Task<KidModel> GetByIdAsync(int kidId);
    Task<KidModel> UpdateAsync(int id, AddKidModel model);
}