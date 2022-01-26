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
    public class CountryPersistence : Controller
    {
        private readonly ApiClient _clientCountry;
        private readonly BlobClient _blobClient;

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
        public async Task<Boolean> Post(Country countryView)
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
                Id = countryView.Flag.Id,
                Symbol = blobContainer.Name.ToString(),
                Path = blobContainer.Uri.AbsolutePath.ToString()
            };
            var country = new Country()
            {
                Id = countryView.Id,
                Label = countryView.Label,
                Flag = flag
            };

            await _clientCountry.PostCountry(country);

            return true;
        }
        public async Task<Boolean> Put(Country country, int? Id)
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
                Id = country.Flag.Id,
                Symbol = blobContainer.Name.ToString(),
                Path = blobContainer.Uri.AbsolutePath.ToString()
            };
            var newCountry = new Country()
            {
                Id = country.Id,
                Label = country.Label,
                Flag = flag
            };

            await _clientCountry.PutCountry(newCountry, Id);

            return true;
        }
        public async Task<Boolean> Delete(int? Id)
        {
            var deleteCountry = await _clientCountry.DeleteCountry(Id);

            if (deleteCountry.IsSuccessStatusCode)
            {
                await deleteCountry.Content.ReadAsAsync<Country>();
                return true;
            }

            return false;
        }
    }
}