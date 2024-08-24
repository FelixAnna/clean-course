using Entities.Entities;
using Shared.Models;

namespace Services.BookCategoryWords
{
    public class WordModel : BaseWordModel
    {
        public WordModel() { }
        public WordModel(WordEntity entity)
        {
            WordId = entity.WordId;
            BookId = entity.BookId;
            Content = entity.Content;
            Explanation = entity.Explanation;
            Details = entity.Details;
            Unit = entity.Unit;
        }
    }
}
