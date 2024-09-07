using Entities.Entities;

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
    }
}
