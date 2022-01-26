using Library.Models.Perfil;
using System;
using System.Collections.Generic;

namespace Library.Context.Perfil.Person
{
    public class ThrowPerson : InterfacePerson
    {
        public IEnumerable<PersonDomain> List()
        {
            throw new NotImplementedException();
        }
        public PersonDomain Get(int? Id)
        {
            throw new NotImplementedException();
        }
        public void Post(PersonDomain personDomain)
        {
            throw new NotImplementedException();
        }
        public void Put(PersonDomain personDomain, int? Id)
        {
            throw new NotImplementedException();
        }
        public void Delete(int? Id)
        {
            throw new NotImplementedException();
        }
    }
}
