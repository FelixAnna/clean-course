namespace Entities.Entities;

public class BookCategoryEntity
{
    public int Id { get; set; }

    public string CategoryName { get; set; }

    public string SharedCode { get; set; }

    public int Year { get; set; }

    public string Grade { get; set; }

    public string Semester { get; set; }

    public bool Selected { get; set; }

}
