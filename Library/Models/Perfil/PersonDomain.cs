using System;

namespace Library.Models.Perfil
{
    public class PersonDomain
    {
        /*public PersonDomain(int Id = 0, string FirstName = null, string LastName = null, int Age = 0, DateTime Birthday = default, PicturesDomain picture = null, ContactsDomain contacts = null, int CountryId = 0)
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
        public PicturesDomain Picture { get; set; }
        public ContactsDomain Contacts { get; set; }
        public int CountryId { get; set; }
    }
}
