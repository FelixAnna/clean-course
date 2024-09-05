using Services.Words.Models;

namespace Services.CheckingHistories.Models
{
    public class WordCheckingHistoriesModel : AddWordModel
    {
        public int Id { get; set; }
        public IList<string>? CheckingRecords { get; set; }

        public WordCheckingHistoriesModel() { }

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
    public static class AddWordHistoryModelConvertor
    {
        const int contentIndex = 1;
        const int explanationIndex = contentIndex + 1;
        const int detailsIndex = contentIndex + 2;
        const int UnitIndex = contentIndex + 3;
        const int CourseIndex = contentIndex + 4;
        const int HistoryIndex = contentIndex + 5;

        public static bool IsValid(string[] values)
        {
            if (values.Length < UnitIndex + 1 || !int.TryParse(values[UnitIndex], out int unit))
            {
                return false;
            }

            return true;
        }

        public static WordCheckingHistoriesModel FromLine(int bookId, string[] values)
        {
            var model = new WordCheckingHistoriesModel()
            {
                BookId = bookId,
                Content = values[contentIndex],
                Explanation = values[explanationIndex],
                Details = values[detailsIndex],
                Unit = int.Parse(values[UnitIndex]),
            };

            if (values.Length > CourseIndex && !string.IsNullOrEmpty(values[CourseIndex]))
            {
                model.BookId = int.Parse(values[CourseIndex]);
            }

            if (values.Length > HistoryIndex)
            {
                model.CheckingRecords = values.Skip(HistoryIndex).Where(x => !string.IsNullOrEmpty(x)).ToArray();
            }

            return model;
        }

    }
}
