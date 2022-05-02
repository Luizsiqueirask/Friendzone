using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using web_viewer.Helper;
using web_viewer.Models.Perfil;
using web_viewer.Models.Places;

namespace web_viewer.Controllers
{
    public class PersonController : Controller
    {
        private readonly ApiClient _clientPerson;
        private readonly BlobClient _blobClient;
        internal readonly string directoryPath = @"../../Storage/Person/";

        public PersonController()
        {
            _clientPerson = new ApiClient();
            _blobClient = new BlobClient();
        }

        // GET: Person
        public async Task<ActionResult> Index()
        {
            var allPerson = await _clientPerson.GetPerson();
            var allCountries = await _clientPerson.GetCountry();
            var containerPersonCountry = new List<PersonCountries>();

            if (allPerson.IsSuccessStatusCode)
            {
                var people = await allPerson.Content.ReadAsAsync<IEnumerable<Person>>();
                var countries = await allCountries.Content.ReadAsAsync<IEnumerable<Country>>();

                if (allCountries.IsSuccessStatusCode)
                {
                    foreach (var person in people)
                    {
                        foreach (var country in countries)
                        {
                            if (person.CountryId == country.Id)
                            {
                                // Together models from Person and Country
                                var personCountries = new PersonCountries()
                                {
                                    People = person,
                                    Countries = country,
                                    CountriesSelect = new List<SelectListItem>() {
                                    new SelectListItem() {
                                        Value = person.Id.ToString(),
                                        Text = person.FirstName,
                                        Selected = country.Id == person.CountryId
                                        }
                                    }
                                };
                                containerPersonCountry.Add(personCountries);
                            }
                        }
                    }
                }
                return View(containerPersonCountry);
            }
            return View(new List<PersonCountries>());
        }

        // GET: Person/Details/5
        public async Task<ActionResult> Details(int? Id)
        {
            var people = await _clientPerson.GetPersonById(Id);
            var personCountries = new PersonCountries();

            if (people.IsSuccessStatusCode)
            {
                var person = await people.Content.ReadAsAsync<Person>();
                var countries = await _clientPerson.GetCountryById(person.CountryId);
                var country = await countries.Content.ReadAsAsync<Country>();

                if (countries.IsSuccessStatusCode)
                {
                    personCountries = new PersonCountries()
                    {
                        People = person,
                        Countries = country,
                        CountrySelect = new SelectListItem()
                        {
                            Value = country.Id.ToString(),
                            Text = country.Label,
                            Selected = country.Id == person.CountryId
                        }
                    };
                }
                return View(personCountries);
            }
            return View(new PersonCountries());
        }

        // GET: Person/Create
        public async Task<ActionResult> Create()
        {
            var allCountries = await _clientPerson.GetCountry();

            if (allCountries.IsSuccessStatusCode)
            {
                var countries = await allCountries.Content.ReadAsAsync<IEnumerable<Country>>();
                var personCountry = new PersonCountry();
                var selectCountryList = new List<SelectListItem>();

                foreach (var country in countries)
                {
                    var selectCountry = new SelectListItem()
                    {
                        Value = country.Id.ToString(),
                        Text = country.Label,
                        Selected = country.Id == personCountry.CountryId
                    };
                    selectCountryList.Add(selectCountry);
                }
                personCountry.CountrySelect = selectCountryList;
                return View(personCountry);
            }
            return View(new PersonCountry());
        }

        // POST: Person/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Person person)
        {
            HttpFileCollectionBase httpFileCollection = Request.Files;
            HttpPostedFileBase postedFileBase = httpFileCollection[0];

            try
            {
                if (ModelState.IsValid)
                {
                    await _blobClient.SetupCloudBlob();

                    var pictureNameBlob = _blobClient.GetRandomBlobName(httpFileCollection[0].FileName);
                    var picturePathblob = _blobClient._blobContainer.GetBlockBlobReference(pictureNameBlob);
                    await picturePathblob.UploadFromStreamAsync(httpFileCollection[0].InputStream);

                    person.Picture.Symbol = picturePathblob.Name.ToString();
                    person.Picture.Path = picturePathblob.Uri.AbsolutePath.ToString();
                    await _clientPerson.PostPerson(person);

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
                        person.Picture.Symbol = pictureName;
                        person.Picture.Path = picturePath;
                        postedFileBase.SaveAs(picturePath);
                        await _clientPerson.PostPerson(person);

                        return RedirectToAction("Index");
                    }
                }
            }
            return View(new Person());
        }

        // GET: Person/Edit/5
        public async Task<ActionResult> Edit(int? Id)
        {
            var people = await _clientPerson.GetPersonById(Id);
            var allCountries = await _clientPerson.GetCountry();

            if (people.IsSuccessStatusCode && allCountries.IsSuccessStatusCode)
            {
                var countries = await allCountries.Content.ReadAsAsync<IEnumerable<Country>>();
                var person = await people.Content.ReadAsAsync<Person>();

                var personCountry = new PersonCountry()
                {
                    Id = person.Id,
                    FirstName = person.FirstName,
                    LastName = person.LastName,
                    Birthday = person.Birthday,
                    Age = person.Age,
                    Picture = new Pictures
                    {
                        Id = person.Picture.Id,
                        Symbol = person.Picture.Symbol,
                        Path = person.Picture.Path
                    },
                    Contacts = new Contacts()
                    {
                        Id = person.Contacts.Id,
                        Email = person.Contacts.Email,
                        Mobile = person.Contacts.Mobile
                    },
                    CountryId = person.CountryId,
                };

                var selectCountryList = new List<SelectListItem>();

                foreach (var country in countries)
                {
                    var selectCountry = new SelectListItem()
                    {
                        Value = country.Id.ToString(),
                        Text = country.Label,
                        Selected = country.Id == personCountry.CountryId
                    };
                    selectCountryList.Add(selectCountry);
                }
                personCountry.CountrySelect = selectCountryList;
                return View(personCountry);
            }
            return View(new PersonCountry());
        }

        // POST: Person/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Person person, int? Id)
        {
            HttpFileCollectionBase httpFileCollection = Request.Files;
            HttpPostedFileBase postedFileBase = httpFileCollection[0];

            try
            {
                if (ModelState.IsValid)
                {
                    await _blobClient.SetupCloudBlob();

                    var pictureNameBlob = _blobClient.GetRandomBlobName(httpFileCollection[0].FileName);
                    var picturePathblob = _blobClient._blobContainer.GetBlockBlobReference(pictureNameBlob);
                    await picturePathblob.UploadFromStreamAsync(httpFileCollection[0].InputStream);

                    person.Picture.Symbol = picturePathblob.Name.ToString();
                    person.Picture.Path = picturePathblob.Uri.AbsolutePath.ToString();

                    // Thread.Sleep(1000);
                    await _clientPerson.PostPerson(person);
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
                        person.Picture.Symbol = pictureName;
                        person.Picture.Path = picturePath;
                        postedFileBase.SaveAs(picturePath);

                        // Thread.Sleep(1000);
                        await _clientPerson.PutPerson(person, Id);

                        return RedirectToAction("Index");
                    }
                }
            }
            return View(new Person());
        }

        // GET: Person/Delete/5
        public async Task<ActionResult> Delete(int? Id)
        {
            var deletePerson = await _clientPerson.GetPersonById(Id);

            try
            {
                if (deletePerson.IsSuccessStatusCode)
                {
                    var person = await deletePerson.Content.ReadAsAsync<Friends>();
                    return View(person);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSG: {ex.Message}");
            }

            return View(new Person());
        }

        // POST: Person/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int Id)
        {
            try
            {
                var deletePerson = await _clientPerson.DeletePerson(Id);

                if (deletePerson.IsSuccessStatusCode)
                {
                    var person = await deletePerson.Content.ReadAsAsync<Person>();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSG: {ex.Message}");
            }
            return View(new Person());
        }
    }
}
