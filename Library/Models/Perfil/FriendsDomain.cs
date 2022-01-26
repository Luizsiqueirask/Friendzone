using System;

namespace Library.Models.Perfil
{
    public class FriendsDomain
    {
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
