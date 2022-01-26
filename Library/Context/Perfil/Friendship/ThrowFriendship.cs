using Library.Models.Perfil;
using System;
using System.Collections.Generic;

namespace Library.Context.Perfil.Friends
{
    public class ThrowFriendship : InterfaceFriendship
    {
        public IEnumerable<FriendshipDomain> List()
        {
            throw new NotImplementedException();
        }
        public FriendshipDomain Get(int? Id)
        {
            throw new NotImplementedException();
        }
        public void Post(FriendshipDomain friendshipDomain)
        {
            throw new NotImplementedException();
        }
        public void Put(FriendshipDomain friendshipDomain, int? Id)
        {
            throw new NotImplementedException();
        }
        public void Delete(int? Id)
        {
            throw new NotImplementedException();
        }
    }
}
