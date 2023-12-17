namespace Shared.Models
{
    public class BaseWordModel : IComparable<BaseWordModel>, IEquatable<BaseWordModel>
    {
        public int WordId { get; set; }

        public string Course { get; set; }

        public string Content { get; set; }

        public string Explanation { get; set; }

        public bool IsEnglish()
        {
            return Course == "英语" || Course == "English";
        }
        public bool IsChinese()
        {
            return Course == "语文" || Course == "Chinese";
        }

        public int CompareTo(BaseWordModel? other)
        {
            return other?.WordId ?? 0 - this.WordId;
        }

        public bool Equals(BaseWordModel? other)
        {
            return other?.WordId == this.WordId;
        }
    }
}
