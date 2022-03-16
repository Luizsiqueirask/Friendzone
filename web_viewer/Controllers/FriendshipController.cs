using System.Threading.Tasks;
using System.Web.Mvc;
using web_viewer.Models.Perfil;
using web_viewer.Persistence;

namespace web_viewer.Controllers
{
    public class FriendshipController : Controller
    {
        public readonly FriendshipPersistence clientFriendship;

        public FriendshipController()
        {
            clientFriendship = new FriendshipPersistence();
        }

        // GET: Friendship
        public async Task<ActionResult> Index()
        {
            var friendship = await clientFriendship.List();
            return View(friendship);
        }

        // GET: Friendship/Details/5
        public async Task<ActionResult> Details(int? Id)
        {
            var friendship = await clientFriendship.Get(Id);
            return View(friendship);
        }

        // GET: Friendship/Create
        public async Task<ActionResult> Create()
        {
            var friendship = await clientFriendship.Create();
            return View(friendship);
        }

        // POST: Friendship/Create
        [HttpPost]
        public async Task<ActionResult> Create(Friendship friendship)
        {
            try
            {
                // TODO: Add insert logic here
                await clientFriendship.Post(friendship);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new Friendship());
            }
        }

        // GET: Friendship/Edit/5
        public async Task<ActionResult> Edit(int? Id)
        {
            var friendship = await clientFriendship.Get(Id);
            return View(friendship);
        }

        // POST: Friendship/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(Friendship friendship, int? Id)
        {
            try
            {
                // TODO: Add update logic here
                await clientFriendship.Put(friendship, Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new Friendship());
            }
        }

        // GET: Friendship/Delete/5
        public async Task<ActionResult> Delete(int? Id)
        {
            var friendship = await clientFriendship.Delete(Id);
            return View(friendship);
        }

        // POST: Friendship/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int Id)
        {
            try
            {
                // TODO: Add delete logic here
                await clientFriendship.Delete(Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new Friendship());
            }
        }
    }
}
