using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using web_viewer.Models.Places;

namespace web_viewer.Models.Perfil
{
    public class Friendship
    {
        [Required(ErrorMessage = "Informe a pessoa")]
        [DisplayName("Pessoa")]
        public int PersonId { get; set; }
        [Required(ErrorMessage = "Informe o amigo")]
        [DisplayName("Amigos")]
        public int FriendsId { get; set; }
    }
    public class PersonFriend
    {
        [Required(ErrorMessage = "Informe a pessoa")]
        [DisplayName("Pessoa")]
        public int PersonId { get; set; }
        [Required(ErrorMessage = "Informe o amigo")]
        [DisplayName("Amigos")]
        public int FriendsId { get; set; }
        public IEnumerable<SelectListItem> PersonSelect { get; set; }
        public IEnumerable<SelectListItem> FriendsSelect { get; set; }
        public IEnumerable<SelectListItem> CountrySelect { get; set; }
    }

    public class PersonFriends
    {
        // Person
        public Person Person { get; set; }
        public SelectListItem PersonSelect { get; set; }
       
        // Friends
        public Friends Friends { get; set; }
        public SelectListItem FriendSelect { get; set; }
        
        // Country
        public Country Countries { get; set; }
        public SelectListItem CountryPersonSelect { get; set; }
        public SelectListItem CountryFriendsSelect { get; set; }

    }
}