using Library.Models.Places;
using System;
using System.Collections.Generic;

namespace Library.Context.Places.States
{
    public class ThrowStates : InterfaceStates
    {
        public IEnumerable<StateDomain> List()
        {
            throw new NotImplementedException();
        }
        public StateDomain Get(int? Id)
        {
            throw new NotImplementedException();
        }
        public void Post(StateDomain stateDomain)
        {
            throw new NotImplementedException();
        }
        public void Put(StateDomain stateDomain, int? Id)
        {
            throw new NotImplementedException();
        }
        public void Delete(int? Id)
        {
            throw new NotImplementedException();
        }
    }
}
