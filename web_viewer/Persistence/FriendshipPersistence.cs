using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using web_viewer.Helper;
using web_viewer.Models.Perfil;
using web_viewer.Models.Places;

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

        public async Task<IEnumerable<PersonFriends>> List()
        {
            var allFriendship = await _clientFriendship.GetFriendship();
            var allpeople = await _clientFriendship.GetPerson();
            var allfriends = await _clientFriendship.GetFriends();

            var containerFriendship = new List<PersonFriends>();

            if (allpeople.IsSuccessStatusCode && allfriends.IsSuccessStatusCode)
            {               
                var allCountries = await _clientFriendship.GetCountry();
                var personFriends = new PersonFriends();

                if (allCountries.IsSuccessStatusCode)
                {
                    if (allFriendship.IsSuccessStatusCode)
                    {
                        var people = await allpeople.Content.ReadAsAsync<IEnumerable<Person>>();
                        var countries = await allCountries.Content.ReadAsAsync<IEnumerable<Country>>();

                        var friends = await allfriends.Content.ReadAsAsync<IEnumerable<Friends>>();
                        var friendships = await allFriendship.Content.ReadAsAsync<IEnumerable<Friendship>>();

                        foreach (var friendship in friendships)
                        {
                            foreach (var person in people)
                            {
                                foreach (var friend in friends)
                                {
                                    foreach (var country in countries)
                                    {
                                        if (friendship.PersonId.Equals(person.Id) || friendship.FriendsId.Equals(friend.Id))
                                        {
                                            if (person.CountryId.Equals(country.Id) || friend.CountryId.Equals(country.Id))
                                            {
                                                personFriends = new PersonFriends()
                                                {
                                                    // Person
                                                    Person = new Person()
                                                    {
                                                        Id = person.Id,
                                                        FirstName = person.FirstName,
                                                        LastName = person.LastName,
                                                        Birthday = person.Birthday,
                                                        Age = person.Age,
                                                        CountryId = person.CountryId,
                                                        Picture = new Pictures()
                                                        {
                                                            Id = person.Picture.Id,
                                                            Symbol = person.Picture.Symbol,
                                                            Path = person.Picture.Path
                                                        },
                                                        Contacts = new Contacts()
                                                        {
                                                            Id = person.Contacts.Id,
                                                            Email = person.Contacts.Email,
                                                            Mobile = person.Contacts.Mobile
                                                        },
                                                    },
                                                    PersonSelect = new SelectListItem()
                                                    {
                                                        Value = person.Id.ToString(),
                                                        Text = person.FirstName + " " + person.LastName,
                                                        Selected = person.Id.Equals(friendship.PersonId)
                                                    },

                                                    // Friends
                                                    Friends = new Friends()
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
                                                    },
                                                    FriendSelect = new SelectListItem()
                                                    {
                                                        Value = friend.Id.ToString(),
                                                        Text = friend.FirstName + " " + friend.LastName,
                                                        Selected = friend.Id.Equals(friendship.FriendsId)
                                                    },

                                                    // Country
                                                    Countries = new Country()
                                                    {
                                                        Id = country.Id,
                                                        Label = country.Label,
                                                        Flag = new Flag()
                                                        {
                                                            Id = country.Flag.Id,
                                                            Symbol = country.Flag.Symbol,
                                                            Path = country.Flag.Path
                                                        }
                                                    },
                                                    CountryPersonSelect = new SelectListItem()
                                                    {
                                                        Value = country.Id.ToString(),
                                                        Text = country.Label,
                                                        Selected = person.CountryId.Equals(country.Id)
                                                    },

                                                    CountryFriendsSelect = new SelectListItem()
                                                    {
                                                        Value = country.Id.ToString(),
                                                        Text = country.Label,
                                                        Selected = friend.CountryId.Equals(country.Id)
                                                    }
                                                };
                                                containerFriendship.Add(personFriends);
                                            }
                                        }
                                    }
                                }
                            }
                            return containerFriendship;
                        }
                    }
                }
            }
            return new List<PersonFriends>();
        }

        /*public async Task<IEnumerable<PersonFriends>> List()
        {
            var allFriendship = await _clientFriendship.GetFriendship();
            var allPeople = await _clientFriendship.GetPerson();
            var allFriends = await _clientFriendship.GetFriends();
            var allCountries = await _clientFriendship.GetCountry();

            var personFriends = new PersonFriends();
            var personSelect = new List<SelectListItem>();
            var friendsSelect = new List<SelectListItem>();
            var containerFriendship = new List<PersonFriends>();

            if (allFriendship.IsSuccessStatusCode)
            {
                var friendships = await allFriendship.Content.ReadAsAsync<IEnumerable<Friendship>>();

                if (allPeople.IsSuccessStatusCode && allFriends.IsSuccessStatusCode)
                {
                    var people = await allPeople.Content.ReadAsAsync<IEnumerable<Person>>();
                    var friends = await allFriends.Content.ReadAsAsync<IEnumerable<Friends>>();

                    if (allCountries.IsSuccessStatusCode)
                    {
                        var countries = await allCountries.Content.ReadAsAsync<IEnumerable<Country>>();

                        foreach (var friendship in friendships)
                        {
                            foreach (var country in countries)
                            {
                                personFriends.Countries = new Country()
                                {
                                    Id = country.Id,
                                    Label = country.Label,
                                    Flag = new Flag()
                                    {
                                        Id = country.Flag.Id,
                                        Symbol = country.Flag.Symbol,
                                        Path = country.Flag.Path
                                    }
                                };                               

                                foreach (var person in people ?? Enumerable.Empty<Person>())
                                {
                                    personFriends.Person = new Person()
                                    {
                                        Id = person.Id,
                                        FirstName = person.FirstName,
                                        LastName = person.LastName,
                                        Birthday = person.Birthday,
                                        Age = person.Age,
                                        CountryId = person.CountryId,
                                        Picture = new Pictures()
                                        {
                                            Id = person.Picture.Id,
                                            Symbol = person.Picture.Symbol,
                                            Path = person.Picture.Path
                                        },
                                        Contacts = new Contacts()
                                        {
                                            Id = person.Contacts.Id,
                                            Email = person.Contacts.Email,
                                            Mobile = person.Contacts.Mobile
                                        },
                                    };
                                    personFriends.PersonSelect = new SelectListItem()
                                    {
                                        Value = person.Id.ToString(),
                                        Text = person.FirstName + " " + person.LastName,
                                        Selected = person.Id.Equals(friendship.PersonId)
                                    };                                   
                                    personFriends.CountryPersonSelect = new SelectListItem()
                                    {
                                        Value = country.Id.ToString(),
                                        Text = country.Label,
                                        Selected = person.CountryId.Equals(country.Id)
                                    };
                                    containerFriendship.Add(personFriends);
                                }

                                foreach (var friend in friends ?? Enumerable.Empty<Friends>())
                                {
                                    personFriends.Friends = new Friends()
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
                                    personFriends.FriendSelect = new SelectListItem()
                                    {
                                        Value = friend.Id.ToString(),
                                        Text = friend.FirstName + " " + friend.LastName,
                                        Selected = friend.Id.Equals(friendship.FriendsId)
                                    };
                                    personFriends.CountryFriendsSelect = new SelectListItem()
                                    {
                                        Value = country.Id.ToString(),
                                        Text = country.Label,
                                        Selected = friend.CountryId.Equals(country.Id)
                                    };
                                    containerFriendship.Add(personFriends);
                                }
                                return containerFriendship;
                            }
                        }
                    }
                }
            }
            return new List<PersonFriends>();
        }*/

        public async Task<PersonFriends> Get(int? Id)
        {
            var friendships = await _clientFriendship.GetFriendshipById(Id);
            
            if (friendships.IsSuccessStatusCode)
            {
                var friendship = await friendships.Content.ReadAsAsync<Friendship>();

                var people = await _clientFriendship.GetPersonById(friendship.PersonId);
                var friends = await _clientFriendship.GetFriendsById(friendship.FriendsId);
                var allCountries = await _clientFriendship.GetCountry();

                if (people.IsSuccessStatusCode && friends.IsSuccessStatusCode)
                {
                    var person = await people.Content.ReadAsAsync<Person>();
                    var friend = await friends.Content.ReadAsAsync<Friends>();

                    if (allCountries.IsSuccessStatusCode)
                    {
                        var countries = await allCountries.Content.ReadAsAsync<IEnumerable<Country>>();
                        foreach (var country in countries)
                        {
                            var personFriends = new PersonFriends()
                            {
                                // Person
                                Person = new Person()
                                {
                                    Id = person.Id,
                                    FirstName = person.FirstName,
                                    LastName = person.LastName,
                                    Birthday = person.Birthday,
                                    Age = person.Age,
                                    CountryId = person.CountryId,
                                    Picture = new Pictures()
                                    {
                                        Id = person.Picture.Id,
                                        Symbol = person.Picture.Symbol,
                                        Path = person.Picture.Path
                                    },
                                    Contacts = new Contacts()
                                    {
                                        Id = person.Contacts.Id,
                                        Email = person.Contacts.Email,
                                        Mobile = person.Contacts.Mobile
                                    },
                                },
                                PersonSelect = new SelectListItem()
                                {
                                    Value = person.Id.ToString(),
                                    Text = person.FirstName + " " + person.LastName,
                                    Selected = person.Id.Equals(friendship.PersonId)
                                },

                                // Friends
                                Friends = new Friends()
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
                                },
                                FriendSelect = new SelectListItem()
                                {
                                    Value = friend.Id.ToString(),
                                    Text = friend.FirstName + " " + friend.LastName,
                                    Selected = friend.Id.Equals(friendship.FriendsId)
                                },

                                // Country
                                Countries = new Country()
                                {
                                    Id = country.Id,
                                    Label = country.Label,
                                    Flag = new Flag()
                                    {
                                        Id = country.Flag.Id,
                                        Symbol = country.Flag.Symbol,
                                        Path = country.Flag.Path
                                    }
                                },
                                CountryPersonSelect = new SelectListItem()
                                {
                                    Value = country.Id.ToString(),
                                    Text = country.Label,
                                    Selected = person.CountryId.Equals(country.Id)
                                },
                                CountryFriendsSelect = new SelectListItem()
                                {
                                    Value = country.Id.ToString(),
                                    Text = country.Label,
                                    Selected = friend.CountryId.Equals(country.Id)
                                }
                            };
                            return personFriends;
                        }
                    }
                }
            }            
            return new PersonFriends();
        }
        public async Task<PersonFriend> Create()
        {
            var allFriendship = await _clientFriendship.GetFriendship();
            var allPeople = await _clientFriendship.GetPerson();
            var allFriends = await _clientFriendship.GetFriends();

            var personFriend = new PersonFriend();
            var personSelect = new List<SelectListItem>();
            var friendsSelect = new List<SelectListItem>();

            if (allFriendship.IsSuccessStatusCode)
            {
                var friendships = await allFriendship.Content.ReadAsAsync<IEnumerable<Friendship>>();

                if (allPeople.IsSuccessStatusCode && allFriends.IsSuccessStatusCode)
                {
                    var people = await allPeople.Content.ReadAsAsync<IEnumerable<Person>>();
                    var friends = await allFriends.Content.ReadAsAsync<IEnumerable<Friends>>();

                    foreach (var friendship in friendships)
                    {
                        foreach (var person in people ?? Enumerable.Empty<Person>())
                        {
                            var selectPeople = new SelectListItem()
                            {
                                Value = person.Id.ToString(),
                                Text = person.FirstName + " " + person.LastName,
                                Selected = person.Id == friendship.PersonId
                            };
                            personSelect.Add(selectPeople);
                        }

                        personFriend.PersonSelect = personSelect;

                        foreach (var friend in friends ?? Enumerable.Empty<Friends>())
                        {
                            var selectFriends = new SelectListItem()
                            {
                                Value = friend.Id.ToString(),
                                Text = friend.FirstName + " " + friend.LastName,
                                Selected = friend.Id == friendship.FriendsId
                            };
                            friendsSelect.Add(selectFriends);
                        }

                        personFriend.FriendsSelect = friendsSelect;
                        return personFriend;
                    }
                }
            }

            return new PersonFriend();
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
        public async Task<Boolean> Update(Friendship friendship, int? Id)
        {
            var putFriendship = await _clientFriendship.PutFriendship(friendship, Id);

            if (putFriendship.IsSuccessStatusCode)
            {
                await putFriendship.Content.ReadAsAsync<Friendship>();
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
        public async Task<Boolean> Delete(int? Id)
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