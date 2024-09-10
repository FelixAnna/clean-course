using Entities;
using Entities.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Words.Models;
using Services.Words.Repositories;
using Services.CheckingHistories.Models;

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
        var words = courseContext.Words.Include(x=>x.Book).AsQueryable();

        if (request.BookId > 0)
        {
            words = words.Where(x => x.BookId == request.BookId);
        }
        else
        {
            if (request.BookCategoryId > 0)
            {
                var bookIds = courseContext.BookCategoryMappings.Where(x => x.BookCategoryId == request.BookCategoryId).Select(x => x.BookId).ToList();
                words = words.Where(x => bookIds.Contains(x.BookId));
            }
        }

        if (request.Unit > 0)
        {
            words = words.Where(x => x.Unit == request.Unit);
        }

        IList<WordEntity> results = await words.ToListAsync();
        if (request.KidId > 0)
        {
            results = await words.AsNoTracking().Include(x => x.CheckingHistories.Where(y => y.KidId == request.KidId)).ToListAsync();

            if (request is SearchWordAndHistoryCriteria historyRequest)
            {
                if ((ECheckingResult)historyRequest.CheckingResult != ECheckingResult.None)
                {
                    switch ((ECheckingResult)historyRequest.CheckingResult)
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
        }

        if (!string.IsNullOrEmpty(request.Keyword))
        {
            results = results.Where(x =>
            x.Content.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
            || (!string.IsNullOrEmpty(x.Details) && x.Details!.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase))
            || x.Explanation.Contains(request.Keyword, StringComparison.OrdinalIgnoreCase)
            ).ToList();
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
                BookId = model.BookId,
                Content = model.Content,
                Explanation = model.Explanation,
                Details = model.Details,
                Unit = model.Unit,
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
        word.Details = model.Details;
        word.Unit = model.Unit;
        word.BookId = model.BookId;
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

    public async Task UpdateAllAsync(params WordEntity[] entities)
    {
        courseContext.Words.AttachRange(entities);
        courseContext.Words.UpdateRange(entities);
        await courseContext.SaveChangesAsync();
    }
}
