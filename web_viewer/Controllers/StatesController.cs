using System.Threading.Tasks;
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
            var allStates = await clientStates.List();
            return View(allStates);
        }

        // GET: States/Details/5
        public async Task<ActionResult> Details(int Id)
        {
            var getStates = await clientStates.Get(Id);
            return View(getStates);
        }

        // GET: States/Create
        public async Task<ActionResult> Create()
        {
            var allStates = await clientStates.Create();
            return View(allStates);
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
        public async Task<ActionResult> Edit(int Id)
        {
            var getStates = await clientStates.Get(Id);
            return View(getStates);
        }

        // POST: States/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int Id, States states)
        {
            try
            {
                // TODO: Add update logic here
                await clientStates.Put(states, Id);
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
            var getStates = await clientStates.Get(Id);
            return View(getStates);
        }

        // POST: States/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int Id)
        {
            try
            {
                // TODO: Add delete logic here
                await clientStates.Delete(Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new States());
            }
        }
    }
}
