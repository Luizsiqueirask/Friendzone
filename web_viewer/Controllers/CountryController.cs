using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using web_viewer.Helper;
using web_viewer.Models.Places;

namespace web_viewer.Controllers
{
    public class CountryController : Controller
    {
        // https://www.c-sharpcorner.com/forums/how-to-save-image-file-in-folder

        private readonly ApiClient _clientCountry;
        private readonly BlobClient _blobClient;
        internal readonly string directoryPath = @"../Storage/Country/";

        public CountryController()

        {
            _clientCountry = new ApiClient();
            _blobClient = new BlobClient();
        }

        // GET: Country
        public async Task<ActionResult> Index()
        {
            var allCountries = await _clientCountry.GetCountry();
            if (allCountries.IsSuccessStatusCode)
            {
                var Countries = await allCountries.Content.ReadAsAsync<IEnumerable<Country>>();
                return View(Countries);
            }

            return View(new List<Country>());
        }

        // GET: Country/Details/5
        public async Task<ActionResult> Details(int? Id)
        {
            var getCountry = await _clientCountry.GetCountryById(Id);

            if (getCountry.IsSuccessStatusCode)
            {
                var country = await getCountry.Content.ReadAsAsync<Country>();
                return View(country);
            }
            return View(new Country());
        }

        // GET: Country/Create
        public async Task<ActionResult> Create()
        {
            return View(new Country());
        }

        // POST: Country/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Country country)
        {
            HttpFileCollectionBase httpFileCollection = Request.Files;
            HttpPostedFileBase postedFileBase = httpFileCollection[0];

            try
            {
                if (ModelState.IsValid)
                {
                    await _blobClient.SetupCloudBlob();

                    var flagNameBlob = _blobClient.GetRandomBlobName(httpFileCollection[0].FileName);
                    var flagPathblob = _blobClient._blobContainer.GetBlockBlobReference(flagNameBlob);
                    await flagPathblob.UploadFromStreamAsync(httpFileCollection[0].InputStream);

                    country.Flag.Symbol = flagPathblob.Name.ToString();
                    country.Flag.Path = flagPathblob.Uri.AbsolutePath.ToString();
                    await _clientCountry.PostCountry(country);

                    return RedirectToAction("Index");
                }
            }
            catch
            {
                if (ModelState.IsValid)
                {
                    // Create pictute on server
                    var pictureName = Path.GetFileName(httpFileCollection[0].FileName);
                    var rootPath = Server.MapPath(directoryPath);
                    var picturePath = Path.Combine(rootPath, pictureName);

                    // Add picture reference to model and save
                    var PictureExt = Path.GetExtension(pictureName);

                    if (PictureExt.Equals(".jpg") || PictureExt.Equals(".jpeg") || PictureExt.Equals(".png"))
                    {
                        country.Flag.Symbol = pictureName;
                        country.Flag.Path = picturePath;
                        postedFileBase.SaveAs(picturePath);
                        await _clientCountry.PostCountry(country);

                        return RedirectToAction("Index");
                    }
                }
            }
            return View(new Country());
        }

        // GET: Country/Edit/5
        public async Task<ActionResult> Edit(int? Id)
        {
            var countries = await _clientCountry.GetCountryById(Id);

            if (countries.IsSuccessStatusCode)
            {
                var country = await countries.Content.ReadAsAsync<Country>();
                return View(country);
            }
            return View(new Country());
        }

        // POST: Country/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int? Id, Country country)
        {
            HttpFileCollectionBase httpFileCollection = Request.Files;
            HttpPostedFileBase postedFileBase = httpFileCollection[0];

            try
            {
                if (ModelState.IsValid)
                {
                    await _blobClient.SetupCloudBlob();

                    var flagNameBlob = _blobClient.GetRandomBlobName(httpFileCollection[0].FileName);
                    var flagPathblob = _blobClient._blobContainer.GetBlockBlobReference(flagNameBlob);
                    await flagPathblob.UploadFromStreamAsync(httpFileCollection[0].InputStream);

                    country.Flag.Symbol = flagPathblob.Name.ToString();
                    country.Flag.Path = flagPathblob.Uri.AbsolutePath.ToString();
                    await _clientCountry.PutCountry(country, Id);

                    return RedirectToAction("Index");
                }
            }
            catch
            {
                if (ModelState.IsValid)
                {
                    // Create pictute on server
                    var pictureName = Path.GetFileName(httpFileCollection[0].FileName);
                    var rootPath = Server.MapPath(directoryPath);
                    var picturePath = Path.Combine(rootPath, pictureName);

                    // Add picture reference to model and save
                    var PictureExt = Path.GetExtension(pictureName);

                    if (PictureExt.Equals(".jpg") || PictureExt.Equals(".jpeg") || PictureExt.Equals(".png"))
                    {
                        country.Flag.Symbol = pictureName;
                        country.Flag.Path = picturePath;
                        postedFileBase.SaveAs(picturePath);
                        await _clientCountry.PutCountry(country, Id);

                        return RedirectToAction("Index");
                    }
                }
            }
            return View(new Country());
        }

        // GET: Country/Delete/5
        public async Task<ActionResult> Delete(int? Id)
        {
            var deleteCountry = await _clientCountry.DeleteCountry(Id);

            try
            {
                Country country = new Country();

                if (deleteCountry.IsSuccessStatusCode)
                {
                    await deleteCountry.Content.ReadAsAsync<Country>();
                    return View(country);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSG: {ex.Message}");
            }

            return View(new Country());
        }

        // POST: Country/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int Id)
        {
            try
            {
                var deleteCountry = await _clientCountry.DeleteFriends(Id);

                if (deleteCountry.IsSuccessStatusCode)
                {
                    var country = await deleteCountry.Content.ReadAsAsync<Country>();
                    return View(country);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSG: {ex.Message}");
            }
            return View(new Country());
        }
    }
}