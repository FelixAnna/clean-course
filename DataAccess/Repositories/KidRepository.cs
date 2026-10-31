using Entities;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Kids.Models;
using Services.Kids.Repositories;

namespace DataAccess.Repositories;

public class KidRepository(AbstractCourseContext courseContext) : IKidRepository
{
    private readonly AbstractCourseContext courseContext = courseContext;

    public async Task<IEnumerable<KidEntity>> GetAllAsync()
    {
        return await courseContext.Kids.ToListAsync();
    }

    public async Task<KidEntity?> GetByIdAsync(int kidId)
    {
        var kid = await courseContext.Kids.FindAsync(kidId);
        return kid;
    }
    public async Task<KidEntity> AddAsync(AddKidModel model)
    {
        var result = await courseContext.Kids.AddAsync(new KidEntity
        {
            Name = model.Name,
            StartSchoolYear = model.StartSchoolYear,
        });

        await courseContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<KidEntity> UpdateAsync(int id, AddKidModel model)
    {
        var kid = await courseContext.Kids.FirstAsync(x => x.KidId == id);
        kid.Name = model.Name;
        kid.StartSchoolYear = model.StartSchoolYear;
        await courseContext.SaveChangesAsync();
        return kid;
    }

    public async Task<bool> RemoveAsync(int kidId)
    {
        var kid = await courseContext.Kids.Include(x => x.CheckingHistories).FirstOrDefaultAsync(x => x.KidId == kidId);
        if (kid != null)
        {
            courseContext.Kids.Remove(kid);
            await courseContext.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task<bool> SetDefaultAsync(int kidId)
    {
        var allKids = await courseContext.Kids.ToArrayAsync();
        if (allKids.All(kid => kid.KidId != kidId))
        {
            return false;
        }

        foreach (var kid in allKids)
        {
            kid.Selected = kid.KidId == kidId;
        }

        await courseContext.SaveChangesAsync();
        return true;
    }

    public async Task<KidEntity?> GetDefaultAsync()
    {
        var kid = await courseContext.Kids.FirstOrDefaultAsync(x => x.Selected == true);

        return kid;
    }
}
