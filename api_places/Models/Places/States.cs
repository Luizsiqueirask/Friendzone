namespace api_places.Models.Places
{
    public class States
    {
        /*public States(int Id=0, string Label=null, Flag Flag=null, int CountryId=0)
        {
            this.Id = Id;
            this.Label = Label;
            this.Flag = Flag;
            this.CountryId = CountryId;
        }*/

        public int Id { get; set; }
        public string Label { get; set; }
        public Flag Flag { get; set; }
        public int CountryId { get; set; }
    }
}