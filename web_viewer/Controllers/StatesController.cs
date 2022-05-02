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
    public class StatesController : Controller
    {
        private readonly ApiClient _clientStates;
        private readonly BlobClient _blobClient;
        internal readonly string directoryPath = @"../../Storage/States/";

        public StatesController()
        {
            _clientStates = new ApiClient();
            _blobClient = new BlobClient();
        }

        // GET: States
        public async Task<ActionResult> Index()
        {
            var allStates = await _clientStates.GetStates();
            var allCountries = await _clientStates.GetCountry();
            var containerStatesCountries = new List<StatesCountries>();

            if (allStates.IsSuccessStatusCode)
            {
                var states = await allStates.Content.ReadAsAsync<IEnumerable<States>>();
                var countries = await allCountries.Content.ReadAsAsync<IEnumerable<Country>>();

                if (allCountries.IsSuccessStatusCode)
                {
                    foreach (var state in states)
                    {
                        foreach (var country in countries)
                        {
                            // Together models from States and Country
                            var statesCountries = new StatesCountries()
                            {
                                States = state,
                                Countries = country,
                                CountriesSelect = new List<SelectListItem>() {
                                    new SelectListItem() {
                                        Value = state.Id.ToString(),
                                        Text = state.Label,
                                        Selected = state.CountryId == country.Id
                                    }
                                }
                            };
                            containerStatesCountries.Add(statesCountries);
                        }
                        return View(containerStatesCountries);
                    }
                }
            }
            return View(new List<StatesCountries>());
        }

        // GET: States/Details/5
        public async Task<ActionResult> Details(int Id)
        {
            var states = await _clientStates.GetStatesById(Id);
            var statesCountries = new StatesCountries();

            if (states.IsSuccessStatusCode)
            {
                var state = await states.Content.ReadAsAsync<States>();
                var countries = await _clientStates.GetCountryById(state.CountryId);
                var country = await countries.Content.ReadAsAsync<Country>();

                if (countries.IsSuccessStatusCode)
                {
                    statesCountries = new StatesCountries()
                    {
                        States = state,
                        Countries = country,
                        CountrySelect = new SelectListItem()
                        {
                            Value = state.Id.ToString(),
                            Text = state.Label,
                            Selected = country.Id == state.CountryId
                        }
                    };
                }
                return View(statesCountries);
            }
            return View(new StatesCountries());
        }

        // GET: States/Create
        public async Task<ActionResult> Create()
        {
            var allCountries = await _clientStates.GetCountry();

            if (allCountries.IsSuccessStatusCode)
            {
                var countries = await allCountries.Content.ReadAsAsync<IEnumerable<Country>>();
                var stateCountry = new States();
                var selectCountryList = new List<SelectListItem>();

                foreach (var country in countries)
                {
                    var selectCountry = new SelectListItem()
                    {
                        Value = country.Id.ToString(),
                        Text = country.Label,
                        Selected = stateCountry.CountryId == country.Id
                    };
                    selectCountryList.Add(selectCountry);
                    stateCountry.CountriesSelect = selectCountryList;
                }
                return View(stateCountry);
            }
            return View(new States());
        }

        // POST: States/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(States states)
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

                    states.Flag.Symbol = flagPathblob.Name.ToString();
                    states.Flag.Path = flagPathblob.Uri.AbsolutePath.ToString();
                    await _clientStates.PostStates(states);

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
                        states.Flag.Symbol = pictureName;
                        states.Flag.Path = picturePath;
                        postedFileBase.SaveAs(picturePath);
                        await _clientStates.PostStates(states);

                        return RedirectToAction("Index");
                    }
                }
            }
            return View(new States());
        }

        // GET: States/Edit/5
        public async Task<ActionResult> Edit(int? Id)
        {
            var states = await _clientStates.GetStatesById(Id);
            var allCountries = await _clientStates.GetCountry();

            if (states.IsSuccessStatusCode && allCountries.IsSuccessStatusCode)
            {
                var countries = await allCountries.Content.ReadAsAsync<IEnumerable<Country>>();
                var state = await states.Content.ReadAsAsync<States>();

                var stateCountry = new StateCountry()
                {
                    Id = state.Id,
                    Label = state.Label,
                    CountryId = state.CountryId,
                    Flag = new Flag()
                    {
                        Id = state.Flag.Id,
                        Symbol = state.Flag.Symbol,
                        Path = state.Flag.Path
                    }
                };

                var selectCountryList = new List<SelectListItem>();

                foreach (var country in countries)
                {
                    var selectCountry = new SelectListItem()
                    {
                        Value = country.Id.ToString(),
                        Text = country.Label,
                        Selected = country.Id == stateCountry.CountryId
                    };
                    selectCountryList.Add(selectCountry);
                }
                stateCountry.CountriesSelect = selectCountryList;
                return View(stateCountry);
            }
            return View(new StateCountry());
        }

        // POST: States/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? Id, States states)
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

                    states.Flag.Symbol = flagPathblob.Name.ToString();
                    states.Flag.Path = flagPathblob.Uri.AbsolutePath.ToString();
                    await _clientStates.PutStates(states, Id);

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
                        states.Flag.Symbol = pictureName;
                        states.Flag.Path = picturePath;
                        postedFileBase.SaveAs(picturePath);
                        await _clientStates.PutStates(states, Id);

                        return RedirectToAction("Index");
                    }
                }
            }
            return View(new States());
        }

        // GET: States/Delete/5
        public async Task<ActionResult> Delete(int? Id)
        {
            var deleteStates = await _clientStates.DeleteStates(Id);

            if (deleteStates.IsSuccessStatusCode)
            {
                var states = await deleteStates.Content.ReadAsAsync<States>();
                return View(states);
            }

            return View(new States());
        }

        // POST: States/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int Id)
        {
            try
            {
                var deleteStates = await _clientStates.DeleteStates(Id);

                if (deleteStates.IsSuccessStatusCode)
                {
                    var states = await deleteStates.Content.ReadAsAsync<States>();
                    return View(states);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSG: {ex.Message}");
            }
            return View(new States());
        }
    }
}