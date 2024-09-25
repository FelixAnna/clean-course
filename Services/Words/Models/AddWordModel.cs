using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Services.Words.Models;

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
public static class WordFromExcelHelper
{
    private const string splitter = "\t";

    const int contentIndex = 1;
    const int explanationIndex = contentIndex + 1;
    const int detailsIndex = contentIndex + 2;
    const int UnitIndex = contentIndex + 3;
    const int BookIndex = contentIndex + 4;

    public static List<AddWordModel> ParseWords(ImportWordsModel model)
    {
        var tobeInsertedNewWords = new List<AddWordModel>();
        var wordsText = model.Content?.Split('\n').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
        for (int i = 0; i < wordsText?.Length; i++)
        {
            var wordParts = wordsText[i].Split(splitter).Select(x => x.Trim()).ToArray();

            if (IsValid(wordParts))
            {
                var addModel = FromLine(model.BookId, wordParts);
                tobeInsertedNewWords.Add(addModel);
            }
        }

        return tobeInsertedNewWords;
    }


    private static bool IsValid(string[] values)
    {
        if (values.Length < UnitIndex + 1 || !int.TryParse(values[UnitIndex], out _))
        {
            return false;
        }

        return true;
    }

    private static AddWordModel FromLine(int bookId, string[] values)
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

public static class WordFromPdfHelper
{
    public static List<AddWordModel> ParseWords(ImportWordsModel model)
    {
        var tobeInsertedNewWords = new List<AddWordModel>();
        var wordsText = model.Content?.Split(['\n', ' ', '.']).Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
        var currentUnit = 0;
        for (int i = 0; i < wordsText?.Length; i++)
        {
            if (int.TryParse(wordsText[i], out int unit))
            {
                if (unit > currentUnit + 5)
                {
                    //might be page number
                    continue;
                }

                currentUnit = unit;
                continue;
            }
            else
            {
                if (currentUnit == 0) {
                    continue;
                }

                var addModel = new AddWordModel()
                {
                    BookId = model.BookId,
                    Content = wordsText[i],
                    Explanation = "NA",
                    Details = string.Empty,
                    Unit = currentUnit
                };

                tobeInsertedNewWords.Add(addModel);
            }
        }

        return tobeInsertedNewWords;
    }
}
public static class EngWordFromPdfHelper
{
    public static List<AddWordModel> ParseWords(ImportWordsModel model)
    {
        var tobeInsertedNewWords = new List<AddWordModel>();
        var wordsText = model.Content?.Split(['\n', '.']).Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
        var currentUnit = 0;
        for (int i = 0; i < wordsText?.Length; i++)
        {
            if (Regex.IsMatch(wordsText[i], @"(Unit)\s+\b+"))
            {
                currentUnit = int.Parse(wordsText[i].Split(' ').Last());
                continue;
            }
            else
            {
                if (currentUnit == 0 || wordsText[i].Length<1)
                {
                    continue;
                }

                var word = wordsText[i];
                var splitIndex = GetSplitIndex(word);
                while (splitIndex == word.Length - 1) {
                    word += " " + wordsText[++i]; //handle line break
                    splitIndex = GetSplitIndex(word);
                }

                if (splitIndex == -1) {
                    continue;
                }

                var addModel = new AddWordModel()
                {
                    BookId = model.BookId,
                    Content = word[..splitIndex].Trim(),
                    Explanation = word[splitIndex..].Trim(),
                    Details = string.Empty,
                    Unit = currentUnit
                };

                tobeInsertedNewWords.Add(addModel);
            }
        }

        return tobeInsertedNewWords;
    }

    private static int GetSplitIndex(string wordAndMeaning)
    {
        bool dot3dUsed = false;
        var splitIndex = -1;
        for(int i=0; i<wordAndMeaning.Length; i++)
        {
            var ch = wordAndMeaning[i];
            if (ch >= 'a' && ch <= 'z')
            {
                splitIndex = i;
            }

            if (ch >= 'A' && ch <= 'Z')
            {
                splitIndex = i;
            }
            
            if(ch == '…' && !dot3dUsed)
            {
                dot3dUsed = true;
                splitIndex = i;
            }
        }

        if (splitIndex > 0)
        {
            var indexOfNextSpace = wordAndMeaning.Substring(splitIndex).IndexOf(' ');
            if (indexOfNextSpace < 0)
            {
                splitIndex = wordAndMeaning.Length - 1;
            }
            else if (indexOfNextSpace >= 0)
            {
                splitIndex += indexOfNextSpace;
            }
        }

        return splitIndex;
    }
}