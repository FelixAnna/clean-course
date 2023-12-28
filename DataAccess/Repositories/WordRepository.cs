using Entities;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Words;
using Services.Words.Models;

namespace DataAccess.Repositories;

public class WordRepository(AbstractCourseContext courseContext, Func<int> checkingThreshold, Func<int> recentThreshold) : IWordRepository
{
    private readonly Func<int> CheckingThreshold = checkingThreshold;
    private readonly Func<int> RecentThreshold = recentThreshold;
    private readonly AbstractCourseContext courseContext = courseContext;

    public async Task<WordEntity?> GetByIdAsync(int id)
    {
        return await courseContext.Words.FindAsync(id);
    }

    public async Task<IList<WordEntity>> FindAsync(SearchWordsCriteria request)
    {
        var words = courseContext.Words.AsQueryable();

        if (!string.IsNullOrEmpty(request.SharedCode))
        {
            words = words.Where(x => x.SharedCode == request.SharedCode);
        }
        if (request.Unit > 0)
        {
            words = words.Where(x => x.Unit == request.Unit);
        }

        if (!string.IsNullOrEmpty(request.Course))
        {
            words = words.Where(x => x.Course == request.Course);
        }

        IList<WordEntity> results = await words.ToListAsync();
        if (request.KidId > 0)
        {
            results = await words.AsNoTracking().Include(x => x.CheckingHistories.Where(y => y.KidId == request.KidId)).ToListAsync();

            if ((ECheckingResult)request.CheckingResult != ECheckingResult.None)
            {
                switch ((ECheckingResult)request.CheckingResult)
                {
                    case ECheckingResult.Unchecked:
                        results = results.Where(x => !x.CheckingHistories.Any()).ToList();
                        break;
                    case ECheckingResult.Success:
                        results = results.Where(x => x.CheckingHistories.Any() && x.CheckingHistories.All(y => y.IsCorrect)).ToList();
                        break;
                    case ECheckingResult.LastFailed:
                        results = results.Where(x => x.CheckingHistories.Any() && !x.CheckingHistories.Last().IsCorrect).ToList();
                        break;
                    case ECheckingResult.UsedFailed:
                        results = results.Where(x => x.CheckingHistories.Any(y => !y.IsCorrect)).ToList();
                        break;
                    case ECheckingResult.InFrequent:
                        var threshold = CheckingThreshold();
                        results = results.Where(x => x.CheckingHistories.Count <= threshold && x.CheckingHistories.Count > 0).ToList();
                        break;
                    case ECheckingResult.RecentFailed:
                        var threshold2 = RecentThreshold();
                        results = results.Where(x => x.CheckingHistories.Any() && x.CheckingHistories.OrderByDescending(y => y.CreatedTime).Take(threshold2).Any(z => !z.IsCorrect)).ToList();
                        break;
                    default:
                        break;
                }
            }
        }

        if (!string.IsNullOrEmpty(request.Content))
        {
            results = results.Where(x => x.Content.Contains(request.Content, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        if (!string.IsNullOrEmpty(request.Explanation))
        {
            results = results.Where(x => x.Explanation.Contains(request.Explanation, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        return results.ToList();
    }

    public async Task<IEnumerable<WordEntity>> AddAsync(params AddWordModel[] models)
    {
        var results = new List<WordEntity>();
        foreach (var model in models)
        {
            var result = courseContext.Words.Add(new WordEntity
            {
                Course = model.Course,
                Content = model.Content,
                Explanation = model.Explanation,
                Unit = model.Unit,
                SharedCode = model.SharedCode,
            });

            results.Add(result.Entity);
        }

        await courseContext.SaveChangesAsync();
        return results;
    }

    public async Task<WordEntity> UpdateAsync(int id, AddWordModel model)
    {
        var word = await courseContext.Words.FirstAsync(x => x.WordId == id);
        word.Content = model.Content;
        word.Explanation = model.Explanation;
        word.Unit = model.Unit;
        word.Course = model.Course;
        await courseContext.SaveChangesAsync();
        return word;
    }

    public async Task<bool> RemoveAsync(int wordId)
    {
        var word = await courseContext.Words.Include(x => x.CheckingHistories).FirstOrDefaultAsync(x => x.WordId == wordId);
        if (word != null)
        {
            courseContext.Words.Remove(word);
            await courseContext.SaveChangesAsync();
            return true;
        }

        return false;
    }

    public async Task RemoveAllAsync(params WordEntity[] entities)
    {
        courseContext.Words.RemoveRange(entities);
        await courseContext.SaveChangesAsync();
    }
}
