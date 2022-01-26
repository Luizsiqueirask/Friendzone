using api_perfil.Models.Perfil;
using api_perfil.Persistence;
using System.Collections.Generic;
using System.Web.Http;

namespace api_perfil.Controllers
{
    public class FriendshipController : ApiController
    {
        private readonly FriendshipPersistence friendshipPersistence;

        public FriendshipController()
        {
            friendshipPersistence = new FriendshipPersistence();
        }

        // GET: api/Friendship
        public IEnumerable<Friendship> Get()
        {
            return friendshipPersistence.List();
        }

        // GET: api/Friendship/5
        public Friendship Get(int? Id)
        {
            return friendshipPersistence.Get(Id);
        }

        // POST: api/Friendship
        public void Post(Friendship friendship)
        {
            friendshipPersistence.Post(friendship);
        }

        // PUT: api/Friendship/5
        public void Put(Friendship friendship, int? Id)
        {
            friendshipPersistence.Put(friendship, Id);
        }

        // DELETE: api/Friendship/5
        public void Delete(int? Id)
        {
            friendshipPersistence.Delete(Id);
        }
    }
}
