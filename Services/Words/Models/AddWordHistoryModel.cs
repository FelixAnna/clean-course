using Services.CheckingHistories.Models;

namespace Services.Words.Models
{
    public class AddWordHistoryModel : AddWordModel
    {
        public int Id { get; set; }
        public IList<string>? CheckingHistories { get; set; }
        public IList<bool>? History => CheckingHistories?.Select(x => x.Trim() == "1" || string.Equals(x.Trim(), CheckingRemark.Correct.ToString(), StringComparison.OrdinalIgnoreCase)).ToArray();
    }
}
