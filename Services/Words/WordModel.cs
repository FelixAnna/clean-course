using Entities.Entities;
using Services.Books;
using Services.Words.Models;
using Shared.Models;

namespace Services.Words;

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
        Source = entity.Source;
        Details = entity.Details;
        Unit = entity.Unit;
        Extensions = entity.Extensions.Select(x => new WordExtensionModel
        {
            Name = x.Name,
            Content = x.Content,
        }).ToList();

        Book = new BookModel(entity.Book);
    }

    public List<WordExtensionModel> Extensions { get; set; } = [];
}
