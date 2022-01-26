using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using web_viewer.Helper;
using web_viewer.Models.Perfil;

namespace web_viewer.Persistence
{
    public class FriendshipPersistence
    {
        private readonly ApiClient _clientFriendship;
        private readonly BlobClient _blobClient;

        public FriendshipPersistence()
        {
            _clientFriendship = new ApiClient();
            _blobClient = new BlobClient();
        }

        public async Task<IEnumerable<Friendship>> List()
        {
            var listFriendship = await _clientFriendship.GetFriendship();

            if (listFriendship.IsSuccessStatusCode)
            {
                var allFriendship = await listFriendship.Content.ReadAsAsync<IEnumerable<Friendship>>();
                return allFriendship;
            }

            return new List<Friendship>();
        }

        public async Task<Friendship> Get(int? Id)
        {
            var getFriendship = await _clientFriendship.GetFriendshipById(Id);

            if (getFriendship.IsSuccessStatusCode)
            {
                var friendship = await getFriendship.Content.ReadAsAsync<Friendship>();
                return friendship;
            }

            return new Friendship();
        }
        public async Task<Boolean> Post(Friendship friendship)
        {
            var postFriendship = await _clientFriendship.PostFriendship(friendship);

            if (postFriendship.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
        public async Task<Boolean> Put(Friendship friendship, int? Id)
        {
            var putFriendship = await _clientFriendship.PutFriendship(friendship, Id);

            if (putFriendship.IsSuccessStatusCode)
            {
                await putFriendship.Content.ReadAsAsync<Friendship>();
                return true;
            }

            return false;
        }
        public async Task<Boolean> Delete(int Id)
        {

            var deleteFriendship = await _clientFriendship.DeleteFriendship(Id);

            if (deleteFriendship.IsSuccessStatusCode)
            {
                await deleteFriendship.Content.ReadAsAsync<Friendship>();
                return true;
            }

            return false;
        }
    }
}