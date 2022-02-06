using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IEnumerable<StatesCountries>> List()
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
                                        Selected = country.Id == state.CountryId
                                    }
                                }
                            };
                            containerStatesCountries.Add(statesCountries);
                        }
                    }
                }               
                return containerStatesCountries;
            }
            return new List<StatesCountries>();
        }
        public async Task<StatesCountries> Get(int? Id)
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
                return statesCountries;
            }
            return new StatesCountries();
        }
        public async Task<StateCountry> Create()
        {
            var allCountries = await _clientStates.GetCountry();

            if (allCountries.IsSuccessStatusCode)
            {
                var countries = await allCountries.Content.ReadAsAsync<IEnumerable<Country>>();
                var stateCountry = new StateCountry();
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
                stateCountry.CountrySelect = selectCountryList;
                return stateCountry;
            }
            return new StateCountry();
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