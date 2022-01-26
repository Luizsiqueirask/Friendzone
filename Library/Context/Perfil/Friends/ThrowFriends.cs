using Library.Models.Perfil;
using System;
using System.Collections.Generic;

namespace Library.Context.Perfil.Friends
{
    public class ThrowPerson : InterfaceFriends
    {
        public IEnumerable<FriendsDomain> List()
        {
            throw new NotImplementedException();
        }
        public FriendsDomain Get(int? Id)
        {
            throw new NotImplementedException();
        }
        public void Post(FriendsDomain friendDomain)
        {
            throw new NotImplementedException();
        }
        public void Put(FriendsDomain friendDomain, int? Id)
        {
            throw new NotImplementedException();
        }
        public void Delete(int? Id)
        {
            throw new NotImplementedException();
        }
    }
}
