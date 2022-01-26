using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace web_viewer.Models.Perfil
{
    public class Friendship
    {
        /*public Friendship(int PersonId = 0, int FriendsId = 0)
        {
            this.PersonId = PersonId;
            this.FriendsId = FriendsId;
        }*/

        [Required(ErrorMessage = "Informe a pessoa")]
        [DisplayName("Pessoa")]
        public int PersonId { get; set; }
        [Required(ErrorMessage = "Informe o amigo")]
        [DisplayName("Amigos")]
        public int FriendsId { get; set; }
    }
    public class PersonFriends
    {
        public Person People { get; set; }
        public Friends Friends { get; set; }
        //public Country Countries { get; set; }
        public SelectListItem FriendSelect { get; set; }
        public IEnumerable<SelectListItem> FriendsSelect { get; set; }
    }
}