using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using web_viewer.Models.Perfil;
using web_viewer.Models.Places;

namespace web_viewer.Helper
{
    public class ApiClient
    {
        public readonly HttpClient _clientPlace;
        public readonly HttpClient _clientPerfil;
        private readonly List<int> ports = new List<int>() { 62678, 60341 };

        public ApiClient()
        {

            _clientPlace = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:{ports[0]}/")
            };

            _clientPerfil = new HttpClient
            {
                BaseAddress = new Uri($"http://localhost:{ports[1]}/")
            };

            _clientPlace.DefaultRequestHeaders.Accept.Clear();
            _clientPerfil.DefaultRequestHeaders.Accept.Clear();

            var mediaType = new MediaTypeWithQualityHeaderValue("application/json");

            _clientPlace.DefaultRequestHeaders.Accept.Add(mediaType);
            _clientPerfil.DefaultRequestHeaders.Accept.Add(mediaType);
        }

        /*#region Pictures
        public async Task<HttpResponseMessage> GetPictures()
        {
            return await _clientPerfil.GetAsync("api/Pictures");
        }
        public async Task<HttpResponseMessage> GetPicturesById(int? Id)
        {
            return await _clientPerfil.GetAsync($"api/Pictures/{Id}");
        }
        public async Task<HttpResponseMessage> PostPictures(Pictures pictures)
        {
            return await _clientPerfil.PostAsJsonAsync("api/Pictures", pictures);
        }
        public async Task<HttpResponseMessage> PutPictures(Pictures pictures, int? Id)
        {
            return await _clientPerfil.PutAsJsonAsync($"api/Pictures/{pictures.Id.Equals(Id)}", pictures);
        }
        public async Task<HttpResponseMessage> DeletePictures(int? Id)
        {
            return await _clientPerfil.DeleteAsync($"api/Pictures/{Id}");
        }
        #endregion Picrtures        

        #region Flag
        public async Task<HttpResponseMessage> GetFlag()
        {
            return await _clientPlace.GetAsync("api/Flag");
        }
        public async Task<HttpResponseMessage> GetFlagById(int? Id)
        {
            return await _clientPlace.GetAsync($"api/Flag/{Id}");
        }
        public async Task<HttpResponseMessage> PostFlag(Flag flag)
        {
            return await _clientPlace.PostAsJsonAsync("api/Flag", flag);
        }
        public async Task<HttpResponseMessage> PutFlag(Flag flag, int? Id)
        {
            return await _clientPlace.PutAsJsonAsync($"api/Flag/{flag.Id.Equals(Id)}", flag);
        }
        public async Task<HttpResponseMessage> DeleteFlag(int? Id)
        {
            return await _clientPlace.DeleteAsync($"api/Flag/{Id}");
        }
        #endregion Flag

        #region Contacts
        public async Task<HttpResponseMessage> GetContacts()
        {
            return await _clientPerfil.GetAsync("api/Contacts");
        }
        public async Task<HttpResponseMessage> GetContactsById(int? Id)
        {
            return await _clientPerfil.GetAsync($"api/Contacts/{Id}");
        }
        public async Task<HttpResponseMessage> PostContacts(Contacts contacts)
        {
            return await _clientPerfil.PostAsJsonAsync("api/Contacts", contacts);
        }
        public async Task<HttpResponseMessage> PutContacts(Contacts contacts, int? Id)
        {
            return await _clientPerfil.PutAsJsonAsync($"api/Contacts/{contacts.Id.Equals(Id)}", contacts);
        }
        public async Task<HttpResponseMessage> DeleteContacts(int? Id)
        {
            return await _clientPerfil.DeleteAsync($"api/Contacts/{Id}");
        }
        #endregion Contacts*/

        #region Country
        public async Task<HttpResponseMessage> GetCountry()
        {
            return await _clientPlace.GetAsync("api/Country");
        }
        public async Task<HttpResponseMessage> GetCountryById(int? Id)
        {
            return await _clientPlace.GetAsync($"api/Country/{Id}");
        }
        public async Task<HttpResponseMessage> PostCountry(Country country)
        {
            return await _clientPlace.PostAsJsonAsync("api/Country", country);
        }
        public async Task<HttpResponseMessage> PutCountry(Country country, int? Id)
        {
            return await _clientPlace.PutAsJsonAsync($"api/Country/{Id}", country);
        }
        public async Task<HttpResponseMessage> DeleteCountry(int? Id)
        {
            return await _clientPlace.DeleteAsync($"api/Country/{Id}");
        }
        #endregion Country        

        #region States
        public async Task<HttpResponseMessage> GetStates()
        {
            return await _clientPlace.GetAsync("api/States");
        }
        public async Task<HttpResponseMessage> GetStatesById(int? Id)
        {
            return await _clientPlace.GetAsync($"api/States/{Id}");
        }
        public async Task<HttpResponseMessage> PostStates(States states)
        {
            return await _clientPlace.PostAsJsonAsync("api/States", states);
        }
        public async Task<HttpResponseMessage> PutStates(States states, int? Id)
        {
            return await _clientPlace.PutAsJsonAsync($"api/States/{Id}", states);
        }
        public async Task<HttpResponseMessage> DeleteStates(int? Id)
        {
            return await _clientPlace.DeleteAsync($"api/States/{Id}");
        }
        #endregion States

        #region Person
        public async Task<HttpResponseMessage> GetPerson()
        {
            return await _clientPerfil.GetAsync("api/Person");
        }
        public async Task<HttpResponseMessage> GetPersonById(int? Id)
        {
            return await _clientPerfil.GetAsync($"api/Person/{Id}");
        }
        public async Task<HttpResponseMessage> PostPerson(Person person)
        {
            return await _clientPerfil.PostAsJsonAsync("api/Person", person);
        }
        public async Task<HttpResponseMessage> PutPerson(Person person, int? Id)
        {
            return await _clientPerfil.PutAsJsonAsync($"api/Person/{Id}", person);
        }
        public async Task<HttpResponseMessage> DeletPerson(int? Id)
        {
            return await _clientPerfil.DeleteAsync($"api/Person/{Id}");
        }
        #endregion Person

        #region Friends
        public async Task<HttpResponseMessage> GetFriends()
        {
            return await _clientPerfil.GetAsync("api/Friends");
        }
        public async Task<HttpResponseMessage> GetFriendsById(int? Id)
        {
            return await _clientPerfil.GetAsync($"api/Friends/{Id}");
        }
        public async Task<HttpResponseMessage> PostFriends(Friends friends)
        {
            return await _clientPerfil.PostAsJsonAsync("api/Friends", friends);
        }
        public async Task<HttpResponseMessage> PutFriends(Friends friends, int? Id)
        {
            return await _clientPerfil.PutAsJsonAsync($"api/Friends/{Id}", friends);
        }
        public async Task<HttpResponseMessage> DeleteFriends(int? Id)
        {
            return await _clientPerfil.DeleteAsync($"api/Friends/{Id}");
        }
        #endregion Friends

        #region Friendship
        public async Task<HttpResponseMessage> GetFriendship()
        {
            return await _clientPerfil.GetAsync("api/Friendship");
        }
        public async Task<HttpResponseMessage> GetFriendshipById(int? Id)
        {
            return await _clientPerfil.GetAsync($"api/Friendship/{Id}");
        }
        public async Task<HttpResponseMessage> PostFriendship(Friendship friendship)
        {
            return await _clientPerfil.PostAsJsonAsync("api/Friendship", friendship);
        }
        public async Task<HttpResponseMessage> PutFriendship(Friendship friendship, int? Id)
        {
            return await _clientPerfil.PutAsJsonAsync($"api/Friendship/{Id}", friendship);
        }
        public async Task<HttpResponseMessage> DeleteFriendship(int? Id)
        {
            return await _clientPerfil.DeleteAsync($"api/Friendship/{Id}");
        }
        #endregion Friendship
    }
}