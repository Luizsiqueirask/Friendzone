namespace api_places.Models.Places
{
    public class States
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public Flag Flag { get; set; }
        public int CountryId { get; set; }
    }
}