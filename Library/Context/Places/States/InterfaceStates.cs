using Library.Models.Places;
using System.Collections.Generic;

namespace Library.Context.Places.States
{
    public interface InterfaceStates
    {
        IEnumerable<StateDomain> List();
        StateDomain Get(int? Id);
        void Post(StateDomain stateDomain);
        void Put(StateDomain stateDomain, int? Id);
        void Delete(int? Id);
    }
}
