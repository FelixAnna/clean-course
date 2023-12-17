using Entities;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Services.CheckingHistories.Models;
using Services.CheckingHistories.Repositories;

namespace DataAccess.Repositories;

public class CheckingHistoryRepository(AbstractCourseContext courseContext) : ICheckingHistoryRepository
{
    private readonly AbstractCourseContext courseContext = courseContext;

    public async Task<IEnumerable<CheckingHistoryEntity>> GetByWordAndKidAsync(int wordId, int kidId)
    {
        var history = courseContext.CheckingHistories.Where(x => x.KidId == kidId)
            .Where(x => x.WordId == wordId)
            .Include(x => x.Kid)
            .Include(x => x.Word);

        return await history.ToListAsync();
    }

    public async Task<CheckingHistoryEntity> AddAsync(AddCheckingHistoryModel model)
    {
        var result = await courseContext.CheckingHistories.AddAsync(new CheckingHistoryEntity
        {
            WordId = model.WordId,
            KidId = model.KidId,
            IsCorrect = model.IsCorrect,
            Remark = model.Remark,
            CreatedTime = DateTime.Now,
        });
        await courseContext.SaveChangesAsync();
        return result.Entity;
    }

    public async Task<IEnumerable<CheckingHistoryEntity>> AddAsync(params AddCheckingHistoryModel[] models)
    {
        var time = DateTime.Now;
        var historyList = models.GroupBy(x => x.WordId).SelectMany(g =>
        {
            var i = -1 * g.Count();
            return g.Select(x => new CheckingHistoryEntity
            {
                WordId = x.WordId,
                KidId = x.KidId,
                IsCorrect = x.IsCorrect,
                Remark = x.Remark,
                CreatedTime = time.AddDays(i++),
            });
        });

        await courseContext.CheckingHistories.AddRangeAsync(historyList);
        await courseContext.SaveChangesAsync();
        return historyList;
    }

    public async Task<bool> RemoveAsync(int historyId)
    {
        var history = await courseContext.CheckingHistories.FindAsync(historyId);
        if (history != null)
        {
            courseContext.CheckingHistories.Remove(history);
            await courseContext.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task RemoveByWordIdsAsync(int kidId, params int[] wordIds)
    {
        var historyList = courseContext.CheckingHistories.Where(x => wordIds.Contains(x.WordId) && x.KidId == kidId).ToArray();
        if (historyList.Length != 0)
        {
            courseContext.CheckingHistories.RemoveRange(historyList);
            await courseContext.SaveChangesAsync();
        }
    }
}
