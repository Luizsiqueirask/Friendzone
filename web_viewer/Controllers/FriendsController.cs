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
    public class FriendsController : Controller
    {
        private readonly ApiClient _clientFriends;
        private readonly BlobClient _blobClient;
        internal readonly string directoryPath = @"../../Storage/Friends/";

        public FriendsController()
        {
            _clientFriends = new ApiClient();
            _blobClient = new BlobClient();
        }

        // GET: Friends
        public async Task<ActionResult> Index()
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
                                    }}
                                };
                                containerFriendsCountries.Add(friendsCountries);
                            }
                        }
                    }
                }
                return View(containerFriendsCountries);
            }
            return View(new List<FriendsCountries>());
        }

        // GET: Friends/Details/5
        public async Task<ActionResult> Details(int? Id)
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
                return View(friendsCountries);
            }
            return View(new FriendsCountries());
        }

        // GET: Friends/Create
        public async Task<ActionResult> Create()
        {
            var allCountries = await _clientFriends.GetCountry();
            var allStates = await _clientFriends.GetStates();
            var friendsCountry = new FriendsCountry();

            if (allCountries.IsSuccessStatusCode && allStates.IsSuccessStatusCode)
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
                return View(friendsCountry);
            }
            return View(new FriendsCountry());
        }

        // POST: Friends/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Friends friends)
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

                    friends.Picture.Symbol = picturePathblob.Name.ToString();
                    friends.Picture.Path = picturePathblob.Uri.AbsolutePath.ToString();
                    await _clientFriends.PostFriends(friends);

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
                        friends.Picture.Symbol = pictureName;
                        friends.Picture.Path = picturePath;
                        postedFileBase.SaveAs(picturePath);
                        await _clientFriends.PostFriends(friends);

                        return RedirectToAction("Index");
                    }
                }
            }
            return View(new Friends());
        }

        // GET: Friends/Edit/5
        public async Task<ActionResult> Edit(int? Id)
        {
            var friends = await _clientFriends.GetFriendsById(Id);
            var allCountries = await _clientFriends.GetCountry();

            if (friends.IsSuccessStatusCode && allCountries.IsSuccessStatusCode)
            {
                var countries = await allCountries.Content.ReadAsAsync<IEnumerable<Country>>();
                var friend = await friends.Content.ReadAsAsync<Friends>();

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
                return View(friendsCountry);
            }
            return View(new FriendsCountry());
        }

        // POST: Friends/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Friends friends, int? Id)
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

                    friends.Picture.Symbol = picturePathblob.Name.ToString();
                    friends.Picture.Path = picturePathblob.Uri.AbsolutePath.ToString();
                    await _clientFriends.PutFriends(friends, Id);

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
                        friends.Picture.Symbol = pictureName;
                        friends.Picture.Path = picturePath;
                        postedFileBase.SaveAs(picturePath);
                        await _clientFriends.PutFriends(friends, Id);

                        return RedirectToAction("Index");
                    }
                }
            }
            return View(new Friends());
        }

        // GET: Friends/Delete/5
        public async Task<ActionResult> Delete(int? Id)
        {
            var deleteFriends = await _clientFriends.DeleteFriends(Id);

            try
            {
                if (deleteFriends.IsSuccessStatusCode)
                {
                    var friends = await deleteFriends.Content.ReadAsAsync<Friends>();
                    return View(friends);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSG: {ex.Message}");
            }
            return View(new Friends());
        }

        // POST: Friends/Delete/5
        [HttpPost]
        public async Task<ActionResult> Delete(int Id)
        {
            try
            {
                var deleteFriends = await _clientFriends.DeleteFriends(Id);

                if (deleteFriends.IsSuccessStatusCode)
                {
                   var friends =  await deleteFriends.Content.ReadAsAsync<Friends>();
                    return View(friends);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MSG: {ex.Message}");
            }
            return View(new Friends());
        }
    }
}
