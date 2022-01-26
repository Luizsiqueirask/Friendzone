namespace Library.Models.Places
{
    public class CountryDomain
    {
        /*public CountryDomain(int Id = 0, string Label = null, FlagDomain flag = null)
        {
            this.Id = Id;
            this.Label = Label;
            this.Flag = Flag;
        }*/

        public int Id { get; set; }
        public string Label { get; set; }
        public FlagDomain Flag { get; set; }
    }
}
