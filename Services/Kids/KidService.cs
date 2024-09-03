using Services.Kids.Models;
using Services.Kids.Repositories;

namespace Services.Kids;

public class KidService(IKidRepository repository) : IKidService
{
    private readonly IKidRepository repository = repository;

    public async Task<SearchKidResult> GetAllAsync()
    {
        var kids = await repository.GetAllAsync();

        var results = kids.Select(x =>
        {
            return new KidModel(x);
        }).ToList();

        return new SearchKidResult()
        {
            Kids = results,
            Count = results.Count
        };
    }

    public async Task<KidModel> AddKidAsync(AddKidModel model)
    {
        var result = await repository.AddAsync(model);
        return new KidModel(result);
    }

    public async Task<KidModel> UpdateAsync(int id, AddKidModel model)
    {
        var result = await repository.UpdateAsync(id, model);
        return new KidModel(result);
    }

    public async Task<bool> UnRegisterKidAsync(int kidId)
    {
        return await repository.RemoveAsync(kidId);
    }

    public async Task<bool> SetDefaultAsync(int kidId)
    {
        return await repository.SetDefaultAsync(kidId);
    }

    public async Task<KidModel> GetDefaultAsync()
    {
        var result = await repository.GetDefaultAsync();
        return new KidModel(result);
    }

    public async Task<KidModel> GetByIdAsync(int kidId)
    {
        var result = await repository.GetByIdAsync(kidId);
        return new KidModel(result!);
    }
}
