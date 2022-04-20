using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using web_viewer.Helper;
using web_viewer.Models.Places;

namespace web_viewer.Persistence
{
    public class StatesPersistence : Controller
    {
        private readonly ApiClient _clientStates;
        private readonly BlobClient _blobClient;
        private readonly HttpPostedFileBase httpPosted;

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
                                        Selected = state.CountryId == country.Id
                                    }
                                }
                            };
                            containerStatesCountries.Add(statesCountries);
                        }
                        return containerStatesCountries;
                    }
                }
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
        public async Task<States> Create()
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
                return stateCountry;
            }
            return new States();
        }
        public async Task<Boolean> Post(States states)
        {
            HttpFileCollectionBase httpFileCollection = Request.Files;
            FileUpload fileUpload = new FileUpload();

            try
            {
                if (httpPosted != null && httpPosted.ContentLength > 0)
                {
                    await _blobClient.SetupCloudBlob();

                    var getBlobName = _blobClient.GetRandomBlobName(httpPosted.FileName);
                    var blobContainer = _blobClient._blobContainer.GetBlockBlobReference(getBlobName);
                    await blobContainer.UploadFromStreamAsync(httpPosted.InputStream);

                    var _states = new States()
                    {
                        Id = states.Id,
                        Label = states.Label,
                        CountryId = states.CountryId,
                        Flag = new Flag()
                        {
                            Id = states.Flag.Id,
                            Symbol = blobContainer.Name.ToString(),
                            Path = blobContainer.Uri.AbsolutePath.ToString()
                        }
                    };

                    await _clientStates.PostStates(_states);
                    return true;
                }
                return false;
            }
            catch
            {
                var directoryPath = @"~/Images/Flags/States/";
                if (ModelState.IsValid)
                {
                    var FlagName = Path.GetFileName(httpPosted.FileName);
                    var FlagExt = Path.GetExtension(FlagName);
                    if (FlagExt.Equals(".jpg") || FlagExt.Equals(".jpeg") || FlagExt.Equals(".png"))
                    {
                        var FlagPath = Server.MapPath(Path.Combine(directoryPath, FlagName));
                        states.Flag.Symbol = FlagName;
                        states.Flag.Path = FlagPath;

                        httpPosted.SaveAs(states.Flag.Path);
                        await _clientStates.PostStates(states);
                    }
                    return true;
                }
                return false;
            }              
        }
        public async Task<StateCountry> Update(int? Id)
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
                return stateCountry;
            }
            return new StateCountry();
        }
        public async Task<Boolean> Put(States states, int? Id, HttpPostedFileBase httpPosted)
        {
            try
            {
                if (httpPosted != null && httpPosted.ContentLength > 0)
                {
                    await _blobClient.SetupCloudBlob();

                    var getBlobName = _blobClient.GetRandomBlobName(httpPosted.FileName);
                    var blobContainer = _blobClient._blobContainer.GetBlockBlobReference(getBlobName);
                    await blobContainer.UploadFromStreamAsync(httpPosted.InputStream);

                    var _states = new States()
                    {
                        Id = states.Id,
                        Label = states.Label,
                        CountryId = states.CountryId,
                        Flag = new Flag()
                        {
                            Id = states.Flag.Id,
                            Symbol = blobContainer.Name.ToString(),
                            Path = blobContainer.Uri.AbsolutePath.ToString()
                        }
                    };

                    await _clientStates.PutStates(_states, Id);
                    return true;
                }
                return false;
            }
            catch
            {
                var directoryPath = @"~/Images/Flags/States/";
                if (httpPosted != null && httpPosted.ContentLength > 0)
                {
                    var FlagName = Path.GetFileName(httpPosted.FileName);
                    var FlagExt = Path.GetExtension(FlagName);
                    if (FlagExt.Equals(".jpg") || FlagExt.Equals(".jpeg") || FlagExt.Equals(".png"))
                    {
                        var FlagPath = Path.Combine(Server.MapPath(directoryPath), FlagName);

                        var _states = new States()
                        {
                            Id = states.Id,
                            Label = states.Label,
                            Flag = new Flag()
                            {
                                Id = states.Flag.Id,
                                Symbol = FlagName,
                                Path = FlagPath
                            }
                        };

                        httpPosted.SaveAs(_states.Flag.Path);
                        await _clientStates.PutStates(_states, Id);
                    }
                    return true;
                }
                return false;
            }
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
        public async Task<States> Delete(int? Id, States states)
        {
            try
            {
                var deleteStates = await _clientStates.DeleteStates(Id);

                if (deleteStates.IsSuccessStatusCode)
                {
                    await deleteStates.Content.ReadAsAsync<States>();
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