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
            SharedCode = entity.SharedCode;
            Course = entity.Course;
            Content = entity.Content;
            Explanation = entity.Explanation;
            Unit = entity.Unit;
        }
    }
}
