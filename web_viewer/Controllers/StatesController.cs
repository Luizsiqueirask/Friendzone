using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using web_viewer.Models.Places;
using web_viewer.Persistence;

namespace web_viewer.Controllers
{
    public class StatesController : Controller
    {
        private readonly StatesPersistence clientStates;

        public StatesController()
        {
            clientStates = new StatesPersistence();
        }

        // GET: States
        public async Task<ActionResult> Index()
        {
            var states = await clientStates.List();
            return View(states);
        }

        // GET: States/Details/5
        public async Task<ActionResult> Details(int Id)
        {
            var states = await clientStates.Get(Id);
            return View(states);
        }

        // GET: States/Create
        public async Task<ActionResult> Create()
        {
            var states = await clientStates.Create();
            return View(states);
        }

        // POST: States/Create
        [HttpPost]
        public async Task<ActionResult> Create(States states)
        {
            try
            {
                // TODO: Add insert logic here
                await clientStates.Post(states);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new States());
            }
        }

        // GET: States/Edit/5
        public async Task<ActionResult> Edit(int? Id)
        {
            var states = await clientStates.Update(Id);
            return View(states);
        }

        // POST: States/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int Id, States states, HttpPostedFileBase httpPosted)
        {
            try
            {
                // TODO: Add update logic here
                await clientStates.Put(states, Id, httpPosted);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new States());
            }
        }

        // GET: States/Delete/5
        public async Task<ActionResult> Delete(int? Id)
        {
            var states = await clientStates.Delete(Id);
            return View(states);
        }

        // POST: States/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int? Id, States states)
        {
            try
            {
                // TODO: Add delete logic here
                await clientStates.Delete(Id, states);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new States());
            }
        }
    }
}