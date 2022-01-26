namespace Library.Models.Places
{
    public class StateDomain
    {
        /*public StateDomain(int Id = 0, string Label = null, FlagDomain Flag = null, int CountryId = 0)
        {
            this.Id = Id;
            this.Label = Label;
            this.Flag = Flag;
            this.CountryId = CountryId;
        }*/

        public int Id { get; set; }
        public string Label { get; set; }
        public FlagDomain Flag { get; set; }
        public int CountryId { get; set; }
    }
}
