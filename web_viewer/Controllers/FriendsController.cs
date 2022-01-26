using System.Threading.Tasks;
using System.Web.Mvc;
using web_viewer.Models.Perfil;
using web_viewer.Persistence;

namespace web_viewer.Controllers
{
    public class FriendsController : Controller
    {
        private readonly FriendsPersistence clientFriends;

        public FriendsController()
        {
            clientFriends = new FriendsPersistence();
        }

        // GET: Friends
        public async Task<ActionResult> Index()
        {
            var listFriends = await clientFriends.List();
            return View(listFriends);
        }

        // GET: Friends/Details/5
        public async Task<ActionResult> Details(int? Id)
        {
            var getFriends = await clientFriends.Get(Id);
            return View(getFriends);
        }

        // GET: Friends/Create
        public async Task<ActionResult> Create()
        {
            //var getFriends = await clientFriends.List();
            return View(new Friends());
        }

        // POST: Friends/Create
        [HttpPost]
        public async Task<ActionResult> Create(Friends friends)
        {
            try
            {
                // TODO: Add insert logic here
                _ = await clientFriends.Post(friends);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new Friendship());
            }
        }

        // GET: Friends/Edit/5
        public async Task<ActionResult> Edit(int? Id)
        {
            var getFriends = await clientFriends.Get(Id);
            return View(getFriends);
        }

        // POST: Friends/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Friends friends, int? Id)
        {
            try
            {
                // TODO: Add update logic here
                _ = await clientFriends.Put(friends, Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new Friends());
            }
        }

        // GET: Friends/Delete/5
        public async Task<ActionResult> Delete(int? Id)
        {
            var getFriendship = await clientFriends.Get(Id);
            return View(getFriendship);
        }

        // POST: Friends/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int Id)
        {
            try
            {
                // TODO: Add delete logic here
                await clientFriends.Delete(Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new Friends());
            }
        }
    }
}
