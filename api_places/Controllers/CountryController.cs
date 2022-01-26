using api_places.Models.Places;
using api_places.Persistence;
using System.Collections.Generic;
using System.Web.Http;

namespace api_places.Controllers
{
    public class CountryController : ApiController
    {
        private readonly CountryPersistence countryPersistence;

        public CountryController()
        {
            countryPersistence = new CountryPersistence();
        }

        // GET: api/Country
        public IEnumerable<Country> Get()
        {
            return countryPersistence.List();
        }

        // GET: api/Country/5
        public Country Get(int? Id)
        {
            return countryPersistence.Get(Id);
        }

        // POST: api/Country
        public void Post(Country country)
        {
            countryPersistence.Post(country);
        }

        // PUT: api/Country/5
        public void Put(Country country, int? Id)
        {
            countryPersistence.Put(country, Id);
        }

        // DELETE: api/Country/5
        public void Delete(int? Id)
        {
            countryPersistence.Delete(Id);
        }
    }
}
