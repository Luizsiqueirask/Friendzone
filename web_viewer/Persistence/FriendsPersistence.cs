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
    public class FriendsPersistence : Controller
    {
        private readonly ApiClient _clientFriends;
        private readonly BlobClient _blobClient;

        public FriendsPersistence()
        {
            _clientFriends = new ApiClient();
            _blobClient = new BlobClient();
        }

        public async Task<IEnumerable<FriendsCountry>> List()
        {
            var allFriends = await _clientFriends.GetFriends();
            var allCountries = await _clientFriends.GetCountry();
            var containerFriendsCountry = new List<FriendsCountry>();

            if (allFriends.IsSuccessStatusCode)
            {
                var friends = await allFriends.Content.ReadAsAsync<IEnumerable<Friends>>();
                var countries = await allCountries.Content.ReadAsAsync<IEnumerable<Country>>();

                if (allCountries.IsSuccessStatusCode)
                {
                    foreach (var friend in friends)
                    {
                        foreach (var country in countries)
                        {
                            if (friend.CountryId == country.Id)
                            {
                                // Together models from Friends and Country
                                var friendsCountry = new FriendsCountry()
                                {
                                    Friends = friend,
                                    Countries = country,
                                    CountriesSelect = new List<SelectListItem>() {
                                    new SelectListItem() {
                                        Value = friend.Id.ToString(),
                                        Text = friend.FirstName,
                                        Selected = country.Id == friend.CountryId
                                    }
                                }
                                };
                                containerFriendsCountry.Add(friendsCountry);
                            }
                        }
                    }
                }
                return containerFriendsCountry;
            }
            return new List<FriendsCountry>();
        }
        public async Task<FriendsCountry> Get(int? Id)
        {
            var friends = await _clientFriends.GetFriendsById(Id);
            var friendsCountry = new FriendsCountry();

            if (friends.IsSuccessStatusCode)
            {
                var friend = await friends.Content.ReadAsAsync<Friends>();
                var countries = await _clientFriends.GetCountryById(friend.CountryId);
                var country = await countries.Content.ReadAsAsync<Country>();

                if (countries.IsSuccessStatusCode)
                {
                    friendsCountry = new FriendsCountry()
                    {
                        Friends = friend,
                        Countries = country,
                        CountrySelect = new SelectListItem()
                        {
                            Value = country.Id.ToString(),
                            Text = country.Label,
                            Selected = country.Id == friend.CountryId
                        }
                    };
                }
                return friendsCountry;
            }
            return new FriendsCountry();
        }
        public async Task<StatesCountry> Create()
        {
            var allCountries = await _clientFriends.GetCountry();
            var allStates = await _clientFriends.GetStates();
            var statesCountry = new StatesCountry();

            if (allStates.IsSuccessStatusCode)
            {
                if (allCountries.IsSuccessStatusCode)
                {
                    var countries = await allCountries.Content.ReadAsAsync<IEnumerable<Country>>();
                    var selectCountryList = new List<SelectListItem>();

                    foreach (var country in countries)
                    {
                        var selectCountry = new SelectListItem()
                        {
                            Value = country.Id.ToString(),
                            Text = country.Label,
                            Selected = country.Id == statesCountry.Countries.Id
                        };
                        selectCountryList.Add(selectCountry);
                    }
                    statesCountry.CountriesSelect = selectCountryList;
                }
            }
            return statesCountry;
        }
        public async Task<Boolean> Post(Friends friends)
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
                Id = friends.Picture.Id,
                Symbol = blobContainer.Name,
                Path = blobContainer.Uri.AbsolutePath,
            };
            var contact = new Contacts()
            {
                Id = friends.Contacts.Id,
                Email = friends.Contacts.Email,
                Mobile = friends.Contacts.Mobile
            };
            var friend = new Friends()
            {
                Id = friends.Id,
                FirstName = friends.FirstName,
                LastName = friends.LastName,
                Age = friends.Age,
                Birthday = friends.Birthday,
                Contacts = contact,
                Picture = picture,
                CountryId = friends.CountryId
            };

            var _friend = await _clientFriends.PostFriends(friend);

            if (_friend.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
        public async Task<Boolean> Put(Friends friends, int? Id)
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
                Id = friends.Picture.Id,
                Symbol = blobContainer.Name,
                Path = blobContainer.Uri.AbsolutePath,
            };
            var contact = new Contacts()
            {
                Id = friends.Contacts.Id,
                Email = friends.Contacts.Email,
                Mobile = friends.Contacts.Mobile
            };
            var _friends = new Friends()
            {
                Id = friends.Id,
                FirstName = friends.FirstName,
                LastName = friends.LastName,
                Age = friends.Age,
                Birthday = friends.Birthday,
                Contacts = contact,
                Picture = picture,
                CountryId = friends.CountryId
            };

            var postFriendship = await _clientFriends.PutFriends(_friends, Id);

            if (postFriendship.IsSuccessStatusCode)
            {
                await postFriendship.Content.ReadAsAsync<Friendship>();
                return true;
            }

            return false;
        }
        public async Task<Boolean> Delete(int? Id)
        {
            var deleteFriends = await _clientFriends.DeleteFriends(Id);

            if (deleteFriends.IsSuccessStatusCode)
            {
                await deleteFriends.Content.ReadAsAsync<Friends>();
                return true;
            }
            return false;
        }
    }
}