using Library.Models.Perfil;
using System.Collections.Generic;

namespace Library.Context.Perfil.Friends
{
    public interface InterfaceFriendship
    {
        IEnumerable<FriendshipDomain> List();
        FriendshipDomain Get(int? Id);
        void Post(FriendshipDomain friendshipDomain);
        void Put(FriendshipDomain friendshipDomain, int? Id);
        void Delete(int? Id);
    }
}
