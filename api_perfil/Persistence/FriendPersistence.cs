﻿using api_perfil.Models.Perfil;
using Library.Context.Perfil.Friends;
using Library.Models.Perfil;
using System.Collections.Generic;

namespace api_perfil.Persistence
{
    public class FriendPersistence
    {
        public ClassFriends classFriends;

        public FriendPersistence()
        {
            classFriends = new ClassFriends();
        }

        public IEnumerable<Friends> List()
        {
            var listFrindsApi = new List<Friends>();
            var allFriends = classFriends.List();

            if (allFriends != null)
            {
                foreach (var friends in allFriends)
                {
                    var contact = new Contacts()
                    {
                        Id = friends.Contacts.Id,
                        Email = friends.Contacts.Email,
                        Mobile = friends.Contacts.Mobile
                    };
                    var pictures = new Pictures()
                    {
                        Id = friends.Picture.Id,
                        Symbol = friends.Picture.Symbol,
                        Path = friends.Picture.Path
                    };
                    var friend = new Friends()
                    {
                        Id = friends.Id,
                        FirstName = friends.FirstName,
                        LastName = friends.LastName,
                        Birthday = friends.Birthday,
                        CountryId = friends.CountryId,
                        Age = friends.Age,
                        Picture = pictures,
                        Contacts = contact
                    };
                    listFrindsApi.Add(friend);
                }

                return listFrindsApi;
            }
            else
            {
                return null;
            }
        }
        public Friends Get(int? Id)
        {
            var friends = classFriends.Get(Id);

            if (friends.Id.Equals(Id))
            {
                var contact = new Contacts()
                {
                    Id = friends.Contacts.Id,
                    Email = friends.Contacts.Email,
                    Mobile = friends.Contacts.Mobile
                };
                var pictures = new Pictures()
                {
                    Id = friends.Picture.Id,
                    Symbol = friends.Picture.Symbol,
                    Path = friends.Picture.Path
                };
                var friend = new Friends()
                {
                    Id = friends.Id,
                    FirstName = friends.FirstName,
                    LastName = friends.LastName,
                    Birthday = friends.Birthday,
                    Age = friends.Age,
                    Picture = pictures,
                    Contacts = contact,
                    CountryId = friends.CountryId
                };
                return friend;
            }
            else
            {
                return null;
            }
        }
        public void Post(Friends friends)
        {
            var contactDomain = new ContactsDomain()
            {
                Id = friends.Contacts.Id,
                Email = friends.Contacts.Email,
                Mobile = friends.Contacts.Mobile
            };
            var pictureDomain = new PicturesDomain()
            {
                Id = friends.Picture.Id,
                Symbol = friends.Picture.Symbol,
                Path = friends.Picture.Path
            };
            var friendsDomain = new FriendsDomain()
            {
                Id = friends.Id,
                FirstName = friends.FirstName,
                LastName = friends.LastName,
                Birthday = friends.Birthday,
                Age = friends.Age,
                Picture = pictureDomain,
                Contacts = contactDomain,
                CountryId = friends.CountryId
            };

            classFriends.Post(friendsDomain);
        }
        public void Put(Friends friends, int? Id)
        {
            var contactDomain = new ContactsDomain()
            {
                Id = friends.Contacts.Id,
                Email = friends.Contacts.Email,
                Mobile = friends.Contacts.Mobile
            };
            var pictureDomain = new PicturesDomain()
            {
                Id = friends.Picture.Id,
                Symbol = friends.Picture.Symbol,
                Path = friends.Picture.Path
            };
            var friendsDomain = new FriendsDomain()
            {
                Id = friends.Id,
                FirstName = friends.FirstName,
                LastName = friends.LastName,
                Birthday = friends.Birthday,
                Age = friends.Age,
                Picture = pictureDomain,
                Contacts = contactDomain,
                CountryId = friends.CountryId
            };

            classFriends.Put(friendsDomain, Id);
        }
        public void Delete(int? Id)
        {
            classFriends.Delete(Id);
        }
    }
}