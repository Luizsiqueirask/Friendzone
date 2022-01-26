using System;

namespace api_perfil.Models.Perfil
{
    public class Person
    {
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