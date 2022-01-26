namespace api_places.Models.Places
{
    public class Country
    {
        /*public Country(int Id = 0, string Label = null, Flag flag = null)
        {
            this.Id = Id;
            this.Label = Label;
            this.Flag = Flag;
        }*/

        public int Id { get; set; }
        public string Label { get; set; }
        public Flag Flag { get; set; }
    }
}