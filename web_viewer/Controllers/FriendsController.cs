using System.Threading.Tasks;
using System.Web;
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
            var friends = await clientFriends.List();
            return View(friends);
        }

        // GET: Friends/Details/5
        public async Task<ActionResult> Details(int? Id)
        {
            var friends = await clientFriends.Get(Id);
            return View(friends);
        }

        // GET: Friends/Create
        public async Task<ActionResult> Create()
        {
            var friends = await clientFriends.Create();
            return View(friends);
        }

        // POST: Friends/Create
        [HttpPost]
        public async Task<ActionResult> Create(Friends friends, HttpPostedFileBase httpPosted)
        {
            try
            {
                // TODO: Add insert logic here
                _ = await clientFriends.Post(friends, httpPosted);
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
            var friends = await clientFriends.Update(Id);
            return View(friends);
        }

        // POST: Friends/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Friends friends, int? Id, HttpPostedFileBase httpPosted)
        {
            try
            {
                // TODO: Add update logic here
                await clientFriends.Put(friends, Id, httpPosted);
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
            var friends = await clientFriends.Get(Id);
            return View(friends);
        }

        // POST: Friends/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int? Id, Friends friends)
        {
            try
            {
                // TODO: Add delete logic here
                await clientFriends.Delete(Id, friends);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new Friends());
            }
        }
    }
}
