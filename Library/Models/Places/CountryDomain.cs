namespace Library.Models.Places
{
    public class CountryDomain
    { 
        public int Id { get; set; }
        public string Label { get; set; }
        public FlagDomain Flag { get; set; }
    }
}
