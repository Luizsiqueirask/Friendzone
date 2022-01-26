using Library.Models.Perfil;
using System.Collections.Generic;

namespace Library.Context.Perfil.Friends
{
    public interface InterfaceFriends
    {
        IEnumerable<FriendsDomain> List();
        FriendsDomain Get(int? Id);
        void Post(FriendsDomain friendDomain);
        void Put(FriendsDomain friendDomain, int? Id);
        void Delete(int? Id);
    }
}
