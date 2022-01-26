using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using web_viewer.Helper;
using web_viewer.Models.Places;

namespace web_viewer.Persistence
{
    public class StatesPersistence : Controller
    {
        private readonly ApiClient _clientStates;
        private readonly BlobClient _blobClient;

        public StatesPersistence()
        {
            _clientStates = new ApiClient();
            _blobClient = new BlobClient();
        }

        public async Task<IEnumerable<StatesCountry>> List()
        {
            var allStates = await _clientStates.GetStates();
            var allCountries = await _clientStates.GetCountry();
            var containerStatesCountry = new List<StatesCountry>();

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
                            if (state.CountryId == country.Id)
                            {   
                                // Together models from States and Country
                                var statesCountry = new StatesCountry()
                                {
                                    States = state,
                                    Countries = country,
                                    CountriesSelect = new List<SelectListItem>() {
                                    new SelectListItem() {
                                        Value = state.Id.ToString(),
                                        Text = state.Label,
                                        Selected = country.Id == state.CountryId
                                    }
                                }
                                };
                                containerStatesCountry.Add(statesCountry);
                            }
                        }
                    }
                }               
                return containerStatesCountry;
            }
            return new List<StatesCountry>();
        }
        public async Task<StatesCountry> Get(int? Id)
        {
            var allStates = await _clientStates.GetCountryById(Id);

            if (allStates.IsSuccessStatusCode)
            {
                var states = await allStates.Content.ReadAsAsync<States>();
                var allCountries = await _clientStates.GetStatesById(states.CountryId);
                var countries = await allCountries.Content.ReadAsAsync<Country>();

                var flagCountry = new Flag()
                {
                    Id = countries.Flag.Id,
                    Symbol = countries.Flag.Symbol,
                    Path = countries.Flag.Path
                };
                var country = new Country()
                {
                    Id = states.Id,
                    Label = states.Label,
                    Flag = flagCountry
                };
                var flagStates = new Flag()
                {
                    Id = states.Flag.Id,
                    Symbol = states.Flag.Symbol,
                    Path = states.Flag.Path
                };
                var state = new States()
                {
                    Id = states.Id,
                    Label = states.Label,
                    CountryId = countries.Id,
                    Flag = flagStates
                };
                var statesCountry = new StatesCountry()
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
                return statesCountry;
            }
            return new StatesCountry();
        }
        // public async Task<States> Detail(int? Id) { }
        public async Task<StatesCountry> Create()
        {

            var statesCountry = new StatesCountry();
            var allCountries = await _clientStates.GetCountry();

            if (allCountries.IsSuccessStatusCode)
            {
                var countries = await allCountries.Content.ReadAsAsync<IEnumerable<StatesCountry>>();
                var countrySelectedList = new List<SelectListItem>();
                foreach (var country in countries)
                {
                    var countrySelected = new SelectListItem
                    {
                        Value = country.Countries.Id.ToString(),
                        Text = country.Countries.Label,
                        Selected = country.Countries.Id == statesCountry.States.CountryId
                    };
                    countrySelectedList.Add(countrySelected);
                }
                statesCountry.CountriesSelect = countrySelectedList;
            }
            return statesCountry;
        }
        public async Task<Boolean> Post(States statesView)
        {
            HttpFileCollectionBase requestFile = Request.Files;
            int fileCount = requestFile.Count;

            if (fileCount == 0) { return false; }

            await _blobClient.SetupCloudBlob();

            var getBlobName = _blobClient.GetRandomBlobName(requestFile[0].FileName);
            var blobContainer = _blobClient._blobContainer.GetBlockBlobReference(getBlobName);
            await blobContainer.UploadFromStreamAsync(requestFile[0].InputStream);

            var flag = new Flag()
            {
                Id = statesView.Flag.Id,
                Symbol = blobContainer.Name.ToString(),
                Path = blobContainer.Uri.AbsolutePath.ToString()
            };
            var states = new States()
            {
                Id = statesView.Id,
                Label = statesView.Label,
                CountryId = statesView.CountryId,
                Flag = flag
            };

            var postStates = await _clientStates.PostStates(states);

            if (postStates.IsSuccessStatusCode)
            {
                await postStates.Content.ReadAsAsync<States>();
                return true;
            }

            return false;
        }
        //public async Task<States> Update(int? Id) { }
        public async Task<Boolean> Put(States statesView, int? Id)
        {
            HttpFileCollectionBase requestFile = Request.Files;
            int fileCount = requestFile.Count;

            if (fileCount == 0) { return false; }

            await _blobClient.SetupCloudBlob();

            var getBlobName = _blobClient.GetRandomBlobName(requestFile[0].FileName);
            var blobContainer = _blobClient._blobContainer.GetBlockBlobReference(getBlobName);
            await blobContainer.UploadFromStreamAsync(requestFile[0].InputStream);

            var flag = new Flag()
            {
                Id = statesView.Flag.Id,
                Symbol = blobContainer.Name.ToString(),
                Path = blobContainer.Uri.AbsolutePath.ToString()
            };
            var states = new States()
            {
                Id = statesView.Id,
                Label = statesView.Label,
                CountryId = statesView.CountryId,
                Flag = flag
            };

            await _clientStates.PutStates(states, Id);

            var putStates = await _clientStates.PostStates(states);

            if (putStates.IsSuccessStatusCode)
            {
                await putStates.Content.ReadAsAsync<States>();
                return true;
            }

            return false;
        }
        //public async Task<States> Detail(int? Id) { }
        public async Task<States> Delete(int? Id)
        {
            var deleteStates = await _clientStates.DeleteStates(Id);

            try
            {
                States states = new States();

                if (deleteStates.IsSuccessStatusCode)
                {
                    states = await deleteStates.Content.ReadAsAsync<States>();
                    return states;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSG: {ex.Message}");
            }

            return new States();
        }
    }
}