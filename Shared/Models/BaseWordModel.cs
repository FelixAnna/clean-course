﻿namespace Shared.Models;

public abstract class BaseWordModel : IComparable<BaseWordModel>, IEquatable<BaseWordModel>
{
    public int WordId { get; set; }

    public int BookId { get; set; }

    public int Unit { get; set; }

    public string Content { get; set; }

    public string Explanation { get; set; }

    public string? Details { get; set; }

    public string Source { get; set; }


    public bool IsLongText()
    {
        return !string.IsNullOrEmpty(Details) && Details.Length > 8;
    }

    public int CompareTo(BaseWordModel? other)
    {
        return other?.WordId ?? 0 - this.WordId;
    }

    public bool Equals(BaseWordModel? other)
    {
        return other?.WordId == this.WordId;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as BaseWordModel);
    }

    public override int GetHashCode()
    {
        return this.WordId.GetHashCode();
    }

    public static bool operator ==(BaseWordModel left, BaseWordModel right)
    {
        if (left is null)
        {
            return right is null;
        }

        return left.Equals(right);
    }

    public static bool operator !=(BaseWordModel left, BaseWordModel right)
    {
        return !(left == right);
    }

    public static bool operator <(BaseWordModel left, BaseWordModel right)
    {
        return left is null ? right is not null : left.CompareTo(right) < 0;
    }

    public static bool operator <=(BaseWordModel left, BaseWordModel right)
    {
        return left is null || left.CompareTo(right) <= 0;
    }

    public static bool operator >(BaseWordModel left, BaseWordModel right)
    {
        return left is not null && left.CompareTo(right) > 0;
    }

    public static bool operator >=(BaseWordModel left, BaseWordModel right)
    {
        return left is null ? right is null : left.CompareTo(right) >= 0;
    }
}
