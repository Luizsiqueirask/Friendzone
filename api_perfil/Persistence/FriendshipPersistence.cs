using api_perfil.Models.Perfil;
using Library.Context.Perfil.Friends;
using Library.Models.Perfil;
using System.Collections.Generic;

namespace api_perfil.Persistence
{
    public class FriendshipPersistence
    {
        protected ClassFriendship classFriendship;

        public FriendshipPersistence()
        {
            classFriendship = new ClassFriendship();
        }

        public IEnumerable<Friendship> List()
        {
            var allFriendshipApi = new List<Friendship>();
            var allFriendship = classFriendship.List();

            if (allFriendship != null)
            {
                foreach (var friendship in allFriendship)
                {
                    var _friendship = new Friendship()
                    {
                        PersonId = friendship.PersonId,
                        FriendsId = friendship.FriendsId
                    };

                    allFriendshipApi.Add(_friendship);
                }
            }

            return allFriendshipApi;

        }
        public Friendship Get(int? Id)
        {
            var friendship = new Friendship();
            var getFriendship = classFriendship.Get(Id);

            if (getFriendship.PersonId.Equals(Id))
            {
                friendship = new Friendship()
                {
                    PersonId = getFriendship.PersonId,
                    FriendsId = getFriendship.FriendsId
                };
            }

            return friendship;
        }
        public void Post(Friendship friendship)
        {

            var friendshipDomain = new FriendshipDomain()
            {
                PersonId = friendship.PersonId,
                FriendsId = friendship.FriendsId
            };

            classFriendship.Post(friendshipDomain);
        }
        public void Put(Friendship friendship, int? Id)
        {
            var friendshipDomain = new FriendshipDomain()
            {
                PersonId = friendship.PersonId,
                FriendsId = friendship.FriendsId
            };

            classFriendship.Put(friendshipDomain, Id);
        }
        public void Delete(int? Id)
        {
            classFriendship.Delete(Id);
        }
    }
}
