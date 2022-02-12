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

        public async Task<IEnumerable<FriendsCountries>> List()
        {
            var allFriends = await _clientFriends.GetFriends();
            var allCountries = await _clientFriends.GetCountry();
            var containerFriendsCountries = new List<FriendsCountries>();

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
                                var friendsCountries = new FriendsCountries()
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
                                containerFriendsCountries.Add(friendsCountries);
                            }
                        }
                    }
                }
                return containerFriendsCountries;
            }
            return new List<FriendsCountries>();
        }
        public async Task<FriendsCountries> Get(int? Id)
        {
            var friends = await _clientFriends.GetFriendsById(Id);
            var friendsCountries = new FriendsCountries();

            if (friends.IsSuccessStatusCode)
            {
                var friend = await friends.Content.ReadAsAsync<Friends>();
                var countries = await _clientFriends.GetCountryById(friend.CountryId);
                var country = await countries.Content.ReadAsAsync<Country>();

                if (countries.IsSuccessStatusCode)
                {
                    friendsCountries = new FriendsCountries()
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
                return friendsCountries;
            }
            return new FriendsCountries();
        }
        public async Task<FriendsCountry> Create()
        {
            var allCountries = await _clientFriends.GetCountry();
            var allStates = await _clientFriends.GetStates();
            var friendsCountry = new FriendsCountry();

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
                            Selected = country.Id == friendsCountry.CountryId
                        };
                        selectCountryList.Add(selectCountry);
                    }
                    friendsCountry.CountrySelect = selectCountryList;
                }
                return friendsCountry;
            }
            return new FriendsCountry();
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
        public async Task<FriendsCountry> Update(int? Id)
        {
            var friends = await _clientFriends.GetFriendsById(Id);
            var allCountries = await _clientFriends.GetCountry();

            if (friends.IsSuccessStatusCode && allCountries.IsSuccessStatusCode)
            {
                var friend = await friends.Content.ReadAsAsync<Friends>();
                var countries = await allCountries.Content.ReadAsAsync<IEnumerable<Country>>();

                var friendsCountry = new FriendsCountry()
                {
                    Id = friend.Id,
                    FirstName = friend.FirstName,
                    LastName = friend.LastName,
                    Birthday = friend.Birthday,
                    Age = friend.Age,
                    Picture = new Pictures()
                    {
                        Id = friend.Picture.Id,
                        Symbol = friend.Picture.Symbol,
                        Path = friend.Picture.Path
                    },
                    Contacts = new Contacts()
                    {
                        Id = friend.Contacts.Id,
                        Email = friend.Contacts.Email,
                        Mobile = friend.Contacts.Mobile
                    },
                    CountryId = friend.CountryId
                };

                var selectCountryList = new List<SelectListItem>();

                foreach (var country in countries)
                {
                    var selectCountry = new SelectListItem()
                    {
                        Value = country.Id.ToString(),
                        Text = country.Label,
                        Selected = country.Id == friendsCountry.CountryId
                    };
                    selectCountryList.Add(selectCountry);
                }
                friendsCountry.CountrySelect = selectCountryList;
                return friendsCountry;
            }
            return new FriendsCountry();
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

