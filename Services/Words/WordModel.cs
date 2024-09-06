using Entities.Entities;
using Services.Books;
using Shared.Models;

namespace Services.Words
{
    public class WordModel : BaseWordModel
    {
        public BookModel Book { get; set; }
        public WordModel() { }
        public WordModel(WordEntity entity)
        {
            WordId = entity.WordId;
            BookId = entity.BookId;
            Content = entity.Content;
            Explanation = entity.Explanation;
            Details = entity.Details;
            Unit = entity.Unit;

            Book = new BookModel(entity.Book);
        }
    }
}
