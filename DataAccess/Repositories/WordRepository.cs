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
        return await courseContext.Words
            .Include(x => x.Book)
            .Include(x => x.Extensions)
            .FirstOrDefaultAsync(x => x.WordId == id);
    }

    public async Task<IList<WordEntity>> FindAsync(SearchWordsCriteria request)
    {
        var words = courseContext.Words
            .AsNoTracking()
            .Include(x => x.Book)
            .Include(x => x.Extensions)
            .AsQueryable();

        if (request.BookId > 0)
        {
            words = words.Where(x => x.BookId == request.BookId);
        }
        else
        {
            if (request.BookCategoryId > 0)
            {
                var bookIds = courseContext.BookCategoryMappings.Where(x => x.BookCategoryId == request.BookCategoryId).Select(x => x.BookId);
                words = words.Where(x => bookIds.Contains(x.BookId));
            }
        }

        if (request.Unit > 0)
        {
            words = words.Where(x => x.Unit == request.Unit);
        }

        if (!string.IsNullOrEmpty(request.Source))
        {
            words =words.Where(x =>x.Source == request.Source);
        }

        if (request.KidId > 0)
        {
            var kidId = request.KidId;
            words = words.Include(x => x.CheckingHistories.Where(history => history.KidId == kidId));

            if (request is SearchWordAndHistoryCriteria historyRequest
                && Enum.IsDefined(typeof(ECheckingResult), historyRequest.CheckingResult)
                && (ECheckingResult)historyRequest.CheckingResult != ECheckingResult.None)
            {
                words = FilterByCheckingResult(
                    words,
                    kidId,
                    (ECheckingResult)historyRequest.CheckingResult);
            }
        }

        if (!string.IsNullOrEmpty(request.Keyword))
        {
            var pattern = $"%{EscapeLikePattern(request.Keyword)}%";
            words = words.Where(word =>
                EF.Functions.Like(word.Content, pattern, "\\")
                || (word.Details != null && EF.Functions.Like(word.Details, pattern, "\\"))
                || (word.Explanation != null && EF.Functions.Like(word.Explanation, pattern, "\\"))
                || (word.Source != null && EF.Functions.Like(word.Source, pattern, "\\"))
                || word.Extensions.Any(extension =>
                    EF.Functions.Like(extension.Name, pattern, "\\")
                    || EF.Functions.Like(extension.Content, pattern, "\\")));
        }

        return await words.ToListAsync();
    }

    private IQueryable<WordEntity> FilterByCheckingResult(
        IQueryable<WordEntity> words,
        int kidId,
        ECheckingResult checkingResult)
    {
        return checkingResult switch
        {
            ECheckingResult.Unchecked => words.Where(word =>
                !word.CheckingHistories.Any(history => history.KidId == kidId)),
            ECheckingResult.Success => words.Where(word =>
                word.CheckingHistories.Any(history => history.KidId == kidId)
                && !word.CheckingHistories.Any(history => history.KidId == kidId && !history.IsCorrect)),
            ECheckingResult.LastFailed => words.Where(word =>
                word.CheckingHistories
                    .Where(history => history.KidId == kidId)
                    .OrderByDescending(history => history.CreatedTime)
                    .Select(history => (bool?)history.IsCorrect)
                    .FirstOrDefault() == false),
            ECheckingResult.UsedFailed => words.Where(word =>
                word.CheckingHistories.Any(history => history.KidId == kidId && !history.IsCorrect)),
            ECheckingResult.InFrequent => FilterInfrequentlyChecked(words, kidId),
            ECheckingResult.RecentFailed => FilterRecentFailures(words, kidId),
            _ => words,
        };
    }

    private IQueryable<WordEntity> FilterInfrequentlyChecked(IQueryable<WordEntity> words, int kidId)
    {
        var checkingThreshold = CheckingThreshold();
        return words.Where(word =>
            word.CheckingHistories.Count(history => history.KidId == kidId) <= checkingThreshold
            && word.CheckingHistories.Any(history => history.KidId == kidId));
    }

    private IQueryable<WordEntity> FilterRecentFailures(IQueryable<WordEntity> words, int kidId)
    {
        var recentThreshold = Math.Max(0, RecentThreshold());
        return words.Where(word =>
            word.CheckingHistories
                .Where(history => history.KidId == kidId)
                .OrderByDescending(history => history.CreatedTime)
                .Take(recentThreshold)
                .Any(history => !history.IsCorrect));
    }

    private static string EscapeLikePattern(string value)
    {
        return value
            .Replace("\\", "\\\\")
            .Replace("%", "\\%")
            .Replace("_", "\\_");
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
                Source = model.Source,
                Extensions = model.Extensions
                    .Where(extension => !string.IsNullOrWhiteSpace(extension.Name) || !string.IsNullOrWhiteSpace(extension.Content))
                    .Select(extension => new WordExtensionEntity
                    {
                        Name = extension.Name.Trim(),
                        Content = extension.Content.Trim(),
                    })
                    .ToList(),
            });

            results.Add(result.Entity);
        }

        await courseContext.SaveChangesAsync();
        return results;
    }

    public async Task<WordEntity> UpdateAsync(int id, AddWordModel model)
    {
        var word = await courseContext.Words
            .Include(x => x.Extensions)
            .FirstAsync(x => x.WordId == id);
        word.Content = model.Content;
        word.Explanation = model.Explanation;
        word.Details = model.Details;
        word.Unit = model.Unit;
        word.BookId = model.BookId;
        word.Source = model.Source;
        courseContext.WordExtensions.RemoveRange(word.Extensions);
        word.Extensions = model.Extensions
            .Where(extension => !string.IsNullOrWhiteSpace(extension.Name) || !string.IsNullOrWhiteSpace(extension.Content))
            .Select(extension => new WordExtensionEntity
            {
                WordId = word.WordId,
                Name = extension.Name.Trim(),
                Content = extension.Content.Trim(),
            })
            .ToList();
        await courseContext.SaveChangesAsync();
        return word;
    }

    public async Task<bool> RemoveAsync(int wordId)
    {
        var word = await courseContext.Words
            .Include(x => x.CheckingHistories)
            .Include(x => x.Extensions)
            .FirstOrDefaultAsync(x => x.WordId == wordId);
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
        courseContext.Words.UpdateRange(entities);
        await courseContext.SaveChangesAsync();
    }
}
