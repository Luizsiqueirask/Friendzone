using Library.Models.Perfil;
using System.Collections.Generic;

namespace Library.Context.Perfil.Person
{
    public interface InterfacePerson
    {
        IEnumerable<PersonDomain> List();
        PersonDomain Get(int? Id);
        void Post(PersonDomain personDomain);
        void Put(PersonDomain personDomain, int? Id);
        void Delete(int? Id);
    }
}
