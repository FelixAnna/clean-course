using Services.Words.Models;

namespace Services.CheckingHistories.Models.Admin;

public class ImportCheckingHistoryModel : ImportWordsModel
{
    public int KidId { get; set; }
    public int BookCategoryId { get; set; }
    public int? BookId { get; set; }
}
