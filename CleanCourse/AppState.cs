using Services.Words.Models;
using Services.CheckingHistories.Models;

namespace CleanCourse;

public class AppState
{
    public int KidId { get; set; }
    public string KidName { get; set; }
    public int BookCategoryId { get; set; }
    public string BookCategoryFullName { get; set; }
    public SearchWordsCriteria SearchWordsCriteria { get; set; }
    public SearchWordAndHistoryCriteria SearchWordAndHistoryCriteria { get; set; }

    public int DefaultPageSize
    {
        get => Preferences.Get(nameof(DefaultPageSize), 8); set => Preferences.Set(nameof(DefaultPageSize), value);
    }
    public int DefaultCheckingThreshold
    {
        get => Preferences.Get(nameof(DefaultCheckingThreshold), 3); set => Preferences.Set(nameof(DefaultCheckingThreshold), value);
    }
    public int DefaultRecentThreshold
    {
        get => Preferences.Get(nameof(DefaultRecentThreshold), 3); set => Preferences.Set(nameof(DefaultRecentThreshold), value);
    }

}
