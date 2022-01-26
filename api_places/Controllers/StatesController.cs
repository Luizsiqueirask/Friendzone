using api_places.Models.Places;
using api_places.Persistence;
using System.Collections.Generic;
using System.Web.Http;

namespace api_places.Controllers
{
    public class StatesController : ApiController
    {
        public readonly StatesPersistence statesPersistence;

        public StatesController()
        {
            statesPersistence = new StatesPersistence();
        }

        // GET: api/States
        public IEnumerable<States> Get()
        {
            return statesPersistence.List();
        }

        // GET: api/States/5
        public States Get(int? Id)
        {
            return statesPersistence.Get(Id);
        }

        // POST: api/States
        public void Post(States states)
        {
            statesPersistence.Post(states);
        }

        // PUT: api/States/5
        public void Put(States states, int? Id)
        {
            statesPersistence.Put(states, Id);
        }

        // DELETE: api/States/5
        public void Delete(int? Id)
        {
            statesPersistence.Delete(Id);
        }
    }
}
