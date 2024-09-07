using Services.Books;
using Shared.Models;

namespace Services.CheckingHistories.Models;

public class CheckingWord : BaseWordModel
{
    public BookModel Book { get; set; }
}
