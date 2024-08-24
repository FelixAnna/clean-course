using System.ComponentModel.DataAnnotations;

namespace Services.BookCategoryWords.Models;

public class AddWordModel : AddWordBaseModel
{
    [Required]
    [StringLength(20, ErrorMessage = "Content is too long (>20).")]
    public string Content { get; set; }

    [Required]
    [StringLength(20, ErrorMessage = "Explanation is too long (>20).")]
    public string Explanation { get; set; }

    [StringLength(1000, ErrorMessage = "Details is too long (>1000).")]
    public string Details { get; set; }

    [Required]
    public int Unit { get; set; }
}
public static class AddWordModelConvertor
{
    const int contentIndex = 1;
    const int explanationIndex = contentIndex + 1;
    const int detailsIndex = contentIndex + 2;
    const int UnitIndex = contentIndex + 3;
    const int BookIndex = contentIndex + 4;

    public static bool IsValid(string[] values)
    {
        if (values.Length < UnitIndex + 1 || !int.TryParse(values[UnitIndex], out _))
        {
            return false;
        }

        return true;
    }

    public static AddWordModel FromLine(int bookId, string[] values)
    {
        var model = new AddWordModel()
        {
            BookId = bookId,
            Content = Decode(values[contentIndex]),
            Explanation = Decode(values[explanationIndex]),
            Details = Decode(values[detailsIndex]),
            Unit = int.Parse(values[UnitIndex]),
        };

        if (values.Length > BookIndex && !string.IsNullOrEmpty(values[BookIndex]))
        {
            model.BookId = int.Parse(values[BookIndex]);
        }

        return model;
    }

    private static string Decode(string value)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;

        var decodedValue = value.Replace("\\n", "\n").Trim();
        return decodedValue;
    }

}