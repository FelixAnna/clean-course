using Services.CheckingHistories.Models;

namespace Services.Words.Models
{
    public class AddWordHistoryModel : AddWordModel
    {
        public int Id { get; set; }
        public IList<string>? CheckingRecords { get; set; }
        public IList<CheckingResult> CheckingHistories => CheckingRecords?.Select(x => new CheckingResult(x)).ToArray();

        public record CheckingResult
        {
            public CheckingRemark Remark { get; private set; }
            public DateTime? CreatedTime { get; private set; }
            public CheckingResult(CheckingRemark remark, DateTime? createdTime)
            {
                Remark = remark;
                CreatedTime = createdTime;
            }

            public CheckingResult(string history)
            {
                CheckingRemark remark = CheckingRemark.Incorrect;
                DateTime? createdTime = null;
                var parts = history.Split('|', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 0)
                {
                    if (Enum.TryParse(parts[0], ignoreCase: true, out CheckingRemark mark))
                    {
                        remark = mark;
                    }
                    else if (int.TryParse(parts[0], out int remarkNum))
                    {
                        remark = (CheckingRemark)remarkNum;
                    }
                    else
                    {
                        remark = CheckingRemark.Incorrect;
                    }
                }

                if (parts.Length == 2 && DateTime.TryParse(parts[1], out DateTime dateOfHistory))
                {
                    createdTime = dateOfHistory;
                }

                Remark = remark;
                CreatedTime = createdTime;
            }
        };
    }
}
