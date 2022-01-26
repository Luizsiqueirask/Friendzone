using System;

namespace api_perfil.Models.Perfil
{
    public class Person
    {
        /*public Person(int Id = 0, string FirstName = null, string LastName = null, int Age = 0, DateTime Birthday = default, Pictures picture = null, Contacts contacts = null, int CountryId = 0)
        {
            this.Id = Id;
            this.FirstName = FirstName;
            this.LastName = LastName;
            this.Age = Age;
            this.Birthday = Birthday;
            this.Picture = picture;
            this.Contacts = contacts;
            this.CountryId = CountryId;
        }*/

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime Birthday { get; set; }
        public Pictures Picture { get; set; }
        public Contacts Contacts { get; set; }
        public int CountryId { get; set; }
    }
}