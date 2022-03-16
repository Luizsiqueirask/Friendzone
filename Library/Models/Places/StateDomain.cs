namespace Library.Models.Places
{
    public class StateDomain
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public FlagDomain Flag { get; set; }
        public int CountryId { get; set; }
    }
}
