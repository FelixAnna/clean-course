using System.ComponentModel.DataAnnotations;

namespace Services.Words.Models;

public class AddWordModel
{
    [Required]
    public required string SharedCode { get; set; }

    [Required]
    public string Course { get; set; }

    [Required]
    [StringLength(10, ErrorMessage = "Content is too long.")]
    public string Content { get; set; }

    [Required]
    [StringLength(10, ErrorMessage = "Explanation is too long.")]
    public string Explanation { get; set; }

    [Required]
    public int Unit { get; set; }
    [Required]
    public string Overwrite { get; set; }
    public bool IsOverwrite => Overwrite == "1";
}
public static class AddWordModelConvertor
{
    const int contentIndex = 1;
    const int explanationIndex = contentIndex + 1;
    const int UnitIndex = contentIndex + 2;
    const int CourseIndex = contentIndex + 3;
    const int HistoryIndex = contentIndex + 4;

    public static bool IsValid(string[] values)
    {
        if (values.Length < UnitIndex + 1 || !int.TryParse(values[UnitIndex], out int unit))
        {
            return false;
        }

        return true;
    }

    public static AddWordModel FromLine(string shareCode, string course, string[] values)
    {
        var model = new AddWordModel()
        {
            SharedCode = shareCode,
            Course = course,
            Content = values[contentIndex],
            Explanation = values[explanationIndex],
            Unit = int.Parse(values[UnitIndex]),
        };

        if (values.Length > CourseIndex && !string.IsNullOrEmpty(values[CourseIndex]))
        {
            model.Course = values[CourseIndex];
        }

        return model;
    }

}