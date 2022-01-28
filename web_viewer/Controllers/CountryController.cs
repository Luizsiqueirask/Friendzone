using System.Threading.Tasks;
using System.Web.Mvc;
using web_viewer.Models.Places;
using web_viewer.Persistence;

namespace web_viewer.Controllers
{
    public class CountryController : Controller
    {
        public readonly CountryPersistence clientCountry;

        public CountryController()
        {
            clientCountry = new CountryPersistence();
        }

        // GET: Country
        public async Task<ActionResult> Index()
        {
            var listCountry = await clientCountry.List();
            return View(listCountry);
        }

        // GET: Country/Details/5
        public async Task<ActionResult> Details(int? Id)
        {
            var getCountry = await clientCountry.Get(Id);
            return View(getCountry);
        }

        // GET: Country/Create
        public async Task<ActionResult> Create()
        {
            //var listCountry = await clientCountry.List();
            return View(new Country());
        }

        // POST: Country/Create
        [HttpPost]
        public async Task<ActionResult> Create(Country country)
        {
            try
            {
                // TODO: Add insert logic here
                await clientCountry.Post(country);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new Country());
            }
        }

        // GET: Country/Edit/5
        public async Task<ActionResult> Edit(int? Id)
        {
            var getcountry = await clientCountry.Get(Id);
            return View(getcountry);
        }

        // POST: Country/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int? Id, Country country)
        {
            try
            {
                // TODO: Add update logic here
                await clientCountry.Put(country, Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new Country());
            }
        }

        // GET: Country/Delete/5
        public async Task<ActionResult> Delete(int? Id)
        {
            var getCountry = await clientCountry.Get(Id);
            return View(getCountry);
        }

        // POST: Country/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int Id)
        {
            try
            {
                // TODO: Add delete logic here
                await clientCountry.Delete(Id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(new Country());
            }
        }
    }
}
