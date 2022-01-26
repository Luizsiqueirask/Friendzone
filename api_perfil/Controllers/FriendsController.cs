using api_perfil.Models.Perfil;
using api_perfil.Persistence;
using System.Collections.Generic;
using System.Web.Http;

namespace api_perfil.Controllers
{
    public class FriendsController : ApiController
    {
        private readonly FriendPersistence friendPersistence;
        public FriendsController()
        {
            friendPersistence = new FriendPersistence();
        }

        // GET: api/Friends
        public IEnumerable<Friends> Get()
        {
            return friendPersistence.List();
        }

        // GET: api/Friends/5
        public Friends Get(int? Id)
        {
            return friendPersistence.Get(Id);
        }

        // POST: api/Friends
        public void Post(Friends friends)
        {
            friendPersistence.Post(friends);
        }

        // PUT: api/Friends/5
        public void Put(Friends friends, int? Id)
        {
            friendPersistence.Put(friends, Id);
        }

        // DELETE: api/Friends/5
        public void Delete(int? Id)
        {
            friendPersistence.Delete(Id);
        }
    }
}
