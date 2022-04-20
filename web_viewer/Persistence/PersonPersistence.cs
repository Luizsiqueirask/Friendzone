using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using web_viewer.Helper;
using web_viewer.Models.Perfil;
using web_viewer.Models.Places;

namespace web_viewer.Persistence
{
    public class PersonPersistence : Controller
    {
        private readonly ApiClient _clientPerson;
        private readonly BlobClient _blobClient;

        public PersonPersistence()
        {
            _clientPerson = new ApiClient();
            _blobClient = new BlobClient();
        }

        public async Task<IEnumerable<PersonCountries>> List()
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
                return containerPersonCountry;
            }
            return new List<PersonCountries>();
        }
        public async Task<PersonCountries> Get(int? Id)
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
                return personCountries;
            }
            return new PersonCountries();
        }
        public async Task<PersonCountry> Create()
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
                return personCountry;
            }
            return new PersonCountry();
        }
        public async Task<Person> Post(Person person)
        {
            HttpFileCollectionBase httpFileCollection = Request.Files;
            FileUpload fileUpload = new FileUpload();

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
                }
            }
            catch
            {
                if (ModelState.IsValid)
                {
                    var directoryPath = @"../web_viewer/uploads/pictures/person";
                    // Create pictute on server
                    var pictureName = Path.GetFileName(httpFileCollection[0].FileName);
                    var picturePath = Server.MapPath(Path.Combine(directoryPath, pictureName));

                    // Add Picture reference to model and save
                    var pictureLocalPath = string.Concat(directoryPath, pictureName);
                    var PictureExt = Path.GetExtension(pictureName);

                    if (PictureExt.Equals(".jpg") || PictureExt.Equals(".jpeg") || PictureExt.Equals(".png"))
                    {
                        person.Picture.Symbol = pictureName;
                        person.Picture.Path = pictureLocalPath;
                        fileUpload.SaveAs(picturePath);

                        Debug.WriteLine(person.FirstName);
                        await _clientPerson.PostPerson(person);
                    }
                }
            }
            return new Person();
        }
        public async Task<PersonCountry> Update(int? Id)
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
                return personCountry;
            }
            return new PersonCountry();
        }
        public async Task<Boolean> Put(Person person, int? Id, HttpPostedFileBase httpPosted)
        {
            try
            {
                if (httpPosted != null && httpPosted.ContentLength > 0)
                {
                    await _blobClient.SetupCloudBlob();

                    var getBlobName = _blobClient.GetRandomBlobName(httpPosted.FileName);
                    var blobContainer = _blobClient._blobContainer.GetBlockBlobReference(getBlobName);
                    await blobContainer.UploadFromStreamAsync(httpPosted.InputStream);

                    var _people = new Person()
                    {
                        Id = person.Id,
                        FirstName = person.FirstName,
                        LastName = person.LastName,
                        Age = person.Age,
                        Birthday = person.Birthday,
                        Contacts = new Contacts()
                        {
                            Id = person.Contacts.Id,
                            Email = person.Contacts.Email,
                            Mobile = person.Contacts.Mobile
                        },
                        Picture = new Pictures()
                        {
                            Id = person.Picture.Id,
                            Symbol = blobContainer.Name,
                            Path = blobContainer.Uri.AbsolutePath,
                        },
                        CountryId = person.CountryId
                    };

                    await _clientPerson.PostPerson(_people);
                    return true;
                }
                return false;
            }
            catch
            {
                var directoryPath = @"~/Images/Flags/Perple/";
                if (httpPosted != null && httpPosted.ContentLength > 0)
                {
                    var PicturesName = Path.GetFileName(httpPosted.FileName);
                    var PicturesExt = Path.GetExtension(PicturesName);
                    if (PicturesExt.Equals(".jpg") || PicturesExt.Equals(".jpeg") || PicturesExt.Equals(".png"))
                    {
                        var PicturesPath = Path.Combine(Server.MapPath(directoryPath), PicturesName);

                        var _people = new Person()
                        {
                            Id = person.Id,
                            FirstName = person.FirstName,
                            LastName = person.LastName,
                            Age = person.Age,
                            Birthday = person.Birthday,
                            Contacts = new Contacts()
                            {
                                Id = person.Contacts.Id,
                                Email = person.Contacts.Email,
                                Mobile = person.Contacts.Mobile
                            },
                            Picture = new Pictures()
                            {
                                Id = person.Picture.Id,
                                Symbol = PicturesName,
                                Path = PicturesPath,
                            },
                            CountryId = person.CountryId
                        };

                        httpPosted.SaveAs(_people.Picture.Path);
                        await _clientPerson.PutPerson(_people, Id);
                    }
                    return true;
                }
                return false;
            }
        }
        public async Task<Person> Delete(int? Id)
        {
            var deletePerson = await _clientPerson.GetPersonById(Id);

            try
            {
                Person person = new Person();

                if (deletePerson.IsSuccessStatusCode)
                {
                    await deletePerson.Content.ReadAsAsync<Friends>();
                    return person;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSG: {ex.Message}");
            }

            return new Person();
        }
        public async Task<Person> Delete(int? Id, Person person)
        {
            try
            {
                var deletePerson = await _clientPerson.DeletePerson(Id);

                if (deletePerson.IsSuccessStatusCode)
                {
                    await deletePerson.Content.ReadAsAsync<Person>();
                    return person;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSG: {ex.Message}");
            }
            return new Person();
        }
    }
}