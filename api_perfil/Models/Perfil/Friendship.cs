namespace api_perfil.Models.Perfil
{
    public class Friendship
    {
        /*public Friendship(int PersonId=0, int FriendsId=0)
        {
            this.PersonId = PersonId;
            this.FriendsId = FriendsId;
        }*/

        public int PersonId { get; set; }
        public int FriendsId { get; set; }
    }
}