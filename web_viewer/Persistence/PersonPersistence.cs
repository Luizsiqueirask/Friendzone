using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
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
                                return containerPersonCountry;
                            }
                        }
                    }
                }
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
        public async Task<Boolean> Post(Person person)
        {
            HttpFileCollectionBase requestFile = Request.Files;
            int fileCount = requestFile.Count;

            if (fileCount == 0) { return false; }

            await _blobClient.SetupCloudBlob();

            var getBlobName = _blobClient.GetRandomBlobName(requestFile[0].FileName);
            var blobContainer = _blobClient._blobContainer.GetBlockBlobReference(getBlobName);
            await blobContainer.UploadFromStreamAsync(requestFile[0].InputStream);

            var picture = new Pictures()
            {
                Id = person.Picture.Id,
                Symbol = blobContainer.Name,
                Path = blobContainer.Uri.AbsolutePath,
            };
            var contact = new Contacts()
            {
                Id = person.Contacts.Id,
                Email = person.Contacts.Email,
                Mobile = person.Contacts.Mobile
            };
            var people = new Person()
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Age = person.Age,
                Birthday = person.Birthday,
                Contacts = contact,
                Picture = picture,
                CountryId = person.CountryId
            };

            var _person = await _clientPerson.PostPerson(people);

            if (_person.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
        public async Task<Boolean> Put(Person person, int? Id)
        {
            HttpFileCollectionBase requestFile = Request.Files;
            int fileCount = requestFile.Count;

            if (fileCount == 0) { return false; }

            await _blobClient.SetupCloudBlob();

            var getBlobName = _blobClient.GetRandomBlobName(requestFile[0].FileName);
            var blobContainer = _blobClient._blobContainer.GetBlockBlobReference(getBlobName);
            await blobContainer.UploadFromStreamAsync(requestFile[0].InputStream);

            var picture = new Pictures()
            {
                Id = person.Picture.Id,
                Symbol = blobContainer.Name,
                Path = blobContainer.Uri.AbsolutePath,
            };
            var contact = new Contacts()
            {
                Id = person.Id,
                Email = person.Contacts.Email,
                Mobile = person.Contacts.Mobile
            };
            var people = new Person()
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Age = person.Age,
                Birthday = person.Birthday,
                Contacts = contact,
                Picture = picture,
                CountryId = person.CountryId
            };

            var _person = await _clientPerson.PutPerson(people, Id);

            if (_person.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
        public async Task<Boolean> Delete(int Id)
        {
            var deleteFriends = await _clientPerson.DeletPerson(Id);

            if (deleteFriends.IsSuccessStatusCode)
            {
                await deleteFriends.Content.ReadAsAsync<Friends>();
                return true;
            }

            return false;
        }
    }
}