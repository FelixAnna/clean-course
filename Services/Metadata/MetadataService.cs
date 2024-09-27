using Services.BookCategories.Models;
using Services.Kids.Models;

namespace Services.Metadata;

public static class MetadataService
{
    public static string[] GetDefaultGrades()
    {
        return
        [
            "一年级",
            "二年级",
            "三年级",
            "四年级",
            "五年级",
            "六年级",
            "七年级",
            "八年级",
            "九年级",
        ];
    }

    public static string[] GetDefaultVersions()
    {
        return
        [
            "人教版",
            "苏教版",
        ];
    }
    

    public static string[] GetDefaultSemesters()
    {
        return ["上学期", "下学期"];
    }

    public static Dictionary<string, string> GetDefaultDuplicateOption()
    {
        return new Dictionary<string, string>
        {
            { "0", "跳过" },
            { "1", "覆盖" }
        };
    }
    public static string[] GetDefaultSources()
    {
        return [ "词语表" , "背诵" , "默写" , "扩展词汇","知识点", "生字表", "其它" ];
    }

    public static Dictionary<string, string> GetDefaultImportOption()
    {
        return new Dictionary<string, string>
        {
            { "0", "追加" },
            { "1", "覆盖" }
        };
    }

    public static Dictionary<int, string> GetDefaultUnits(int defaultLen = 30)
    {
        var units = new Dictionary<int, string>
        {
            { 0, "全部" }
        };

        for (var i = 0; i < defaultLen; i++)
        {
            units.Add(i + 1, (i + 1).ToString());
        }

        return units;
    }

    public static string[] GetDefaultCheckingStatuses()
    {
        return ["全部", "没有检查", "检查全对", "上次错误", "部分错误", "较少检查（可在设置里修改）", "最近有错（可在设置里修改）"];
    }

    public static int[] GetDefaultYears(int defaultLen = 10)
    {
        var years = new int[defaultLen];

        var thisYear = DateTime.Now.Year;
        for (var i = 0; i < defaultLen; i++)
        {
            years[i] = thisYear - i;
        }

        return years;
    }

    public static AddKidModel GetDefaultKid()
    {
        var kid = new AddKidModel()
        {
            Name = "宝宝",
            StartSchoolYear = DateTime.Now.Year,
        };

        return kid;
    }

    public static AddBookCategoryModel GetDefaultBookCategory()
    {
        var category = new AddBookCategoryModel()
        {
            CategoryName = "",
        };

        return category;
    }
}
