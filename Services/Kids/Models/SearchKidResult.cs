namespace Services.Kids.Models
{
    public class SearchKidResult
    {
        public IEnumerable<KidModel> Kids { get; set; }

        public int Count { get; set; }
    }
}
