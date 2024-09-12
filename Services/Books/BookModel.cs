using Entities.Entities;
using Services.Words;

namespace Services.Books
{
    public class BookModel(BookEntity entity)
    {
        public int BookId { get; set; } = entity.BookId;

        public string BookName { get; set; } = entity.BookName;

        public string Version { get; set; } = entity.Version;

        public int AuditYear { get; set; } = entity.AuditYear;

        public string Grade { get; set; } = entity.Grade;

        public string Semester { get; set; } = entity.Semester;

        public string FriendlyName => $"{Grade} {BookName} {Semester}({Version})";
        public string ShortName => $"{BookName}";

        public IList<int> Units { get; set; } = entity.Words?.Select(x=>x.Unit).OrderBy(x=>x).Distinct().ToList()??[];

        public bool IsEnglish()
        {
            return IsMatch("英语", "English");
        }

        public bool IsChinese()
        {
            return IsMatch("语文", "Chinese");
        }

        public bool IsMath()
        {
            return IsMatch("数学", "Math");
        }

        private bool IsMatch(params string[] patterns)
        {
            foreach (var pattern in patterns)
            {
                if (FriendlyName.Contains(pattern))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
