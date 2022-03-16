using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using web_viewer.Helper;
using web_viewer.Models.Places;

namespace web_viewer.Persistence
{
    // https://www.c-sharpcorner.com/forums/how-to-save-image-file-in-folder

    public class CountryPersistence : Controller
    {
        private readonly ApiClient _clientCountry;
        private readonly BlobClient _blobClient;
        public HttpPostedFileBase httpPosted;

        public CountryPersistence()
        {
            _clientCountry = new ApiClient();
            _blobClient = new BlobClient();
        }

        public async Task<IEnumerable<Country>> List()
        {
            var listCountries = await _clientCountry.GetCountry();

            if (listCountries.IsSuccessStatusCode)
            {
                var allCountries = await listCountries.Content.ReadAsAsync<IEnumerable<Country>>();
                return allCountries;
            }

            return new List<Country>();
        }
        public async Task<Country> Get(int? Id)
        {
            var getCountry = await _clientCountry.GetCountryById(Id);

            if (getCountry.IsSuccessStatusCode)
            {
                var country = await getCountry.Content.ReadAsAsync<Country>();
                return country;
            }
            return new Country();
        }
        public async Task<Country> Create()
        {
            return new Country();
        }
        public async Task<Boolean> Post(Country country, HttpPostedFileBase httpPosted)
        {
            try
            {
                if (httpPosted != null && httpPosted.ContentLength > 0)
                {
                    await _blobClient.SetupCloudBlob();

                    var getBlobName = _blobClient.GetRandomBlobName(httpPosted.FileName);
                    var blobContainer = _blobClient._blobContainer.GetBlockBlobReference(getBlobName);
                    await blobContainer.UploadFromStreamAsync(httpPosted.InputStream);

                    country.Flag.Symbol = blobContainer.Name.ToString();
                    country.Flag.Path = blobContainer.Uri.AbsolutePath.ToString();

                    await _clientCountry.PostCountry(country);
                    return true;
                }
                return false;
            }
            catch
            {
                var directoryPath = @"~/Images/Flags/Countries/";
                if (httpPosted != null && httpPosted.ContentLength > 0)
                {
                    var FlagName = Path.GetFileName(httpPosted.FileName);
                    var FlagExt = Path.GetExtension(FlagName);
                    if (FlagExt.Equals(".jpg") || FlagExt.Equals(".jpeg") || FlagExt.Equals(".png"))
                    {
                        var FlagPath = Path.Combine(Server.MapPath(directoryPath), FlagName);

                        country.Flag.Symbol = FlagName;
                        country.Flag.Path = FlagPath;

                        httpPosted.SaveAs(country.Flag.Path);
                        await _clientCountry.PostCountry(country);
                    }
                    return true;
                }
                return false;
            }
        }
        public async Task<Country> Update(int? Id)
        {
            var countries = await _clientCountry.GetCountryById(Id);

            if (countries.IsSuccessStatusCode)
            {
                var country = await countries.Content.ReadAsAsync<Country>();
                return country;
            }
            return new Country();
        }
        public async Task<Boolean> Put(Country country, int? Id, HttpPostedFileBase httpPosted)
        {
            try
            {
                if (httpPosted != null && httpPosted.ContentLength > 0)
                {
                    await _blobClient.SetupCloudBlob();

                    var getBlobName = _blobClient.GetRandomBlobName(httpPosted.FileName);
                    var blobContainer = _blobClient._blobContainer.GetBlockBlobReference(getBlobName);
                    await blobContainer.UploadFromStreamAsync(httpPosted.InputStream);

                    var _country = new Country()
                    {
                        Id = country.Id,
                        Label = country.Label,
                        Flag = new Flag()
                        {
                            Id = country.Id,
                            Symbol = blobContainer.Name.ToString(),
                            Path = blobContainer.Uri.AbsolutePath.ToString()
                        }
                    };
                    await _clientCountry.PostCountry(_country);
                    return true;
                }
                return false;
            }
            catch
            {
                var directoryPath = @"~/Friendzone/Library/Content/Flags/Countries/";
                if (httpPosted != null && httpPosted.ContentLength > 0)
                {
                    var FlagName = Path.GetFileName(httpPosted.FileName);
                    var FlagExt = Path.GetExtension(FlagName);
                    if (FlagExt.Equals(".jpg") || FlagExt.Equals(".jpeg") || FlagExt.Equals(".png"))
                    {
                        var FlagPath = Path.Combine(Server.MapPath(directoryPath), FlagName);

                        var _country = new Country()
                        {
                            Id = country.Id,
                            Label = country.Label,
                            Flag = new Flag()
                            {
                                Id = country.Id,
                                Symbol = FlagName,
                                Path = FlagPath
                            }
                        };

                        httpPosted.SaveAs(_country.Flag.Path);
                        await _clientCountry.PostCountry(_country);
                    }
                    return true;
                }
                return false;
            }
        }
        public async Task<Country> Delete(int? Id)
        {
            var deleteCountry = await _clientCountry.DeleteCountry(Id);

            try
            {
                Country country = new Country();

                if (deleteCountry.IsSuccessStatusCode)
                {
                    await deleteCountry.Content.ReadAsAsync<Country>();
                    return country;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSG: {ex.Message}");
            }

            return new Country();
        }
        public async Task<Country> Delete(int? Id, Country country)
        {
            try
            {
                var deleteCountry = await _clientCountry.DeleteFriends(Id);

                if (deleteCountry.IsSuccessStatusCode)
                {
                    await deleteCountry.Content.ReadAsAsync<Country>();
                    return country;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSG: {ex.Message}");
            }
            return new Country();
        }
    }
}