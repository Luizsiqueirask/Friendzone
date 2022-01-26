using api_perfil.Models.Perfil;
using api_perfil.Persistence;
using System.Collections.Generic;
using System.Web.Http;

namespace api_perfil.Controllers
{
    public class PersonController : ApiController
    {
        private readonly PersonPersistence personPersistence;

        public PersonController()
        {
            personPersistence = new PersonPersistence();
        }

        // GET: api/Person
        public IEnumerable<Person> Get()
        {
            return personPersistence.List();
        }

        // GET: api/Person/5
        public Person Get(int? Id)
        {
            return personPersistence.Get(Id);
        }

        // POST: api/Person
        public void Post(Person person)
        {
            personPersistence.Post(person);
        }

        // PUT: api/Person/5
        public void Put(Person person, int? Id)
        {
            personPersistence.Put(person, Id);
        }

        // DELETE: api/Person/5
        public void Delete(int? Id)
        {
            personPersistence.Delete(Id);
        }
    }
}
