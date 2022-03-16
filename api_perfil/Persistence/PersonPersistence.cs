using api_perfil.Models.Perfil;
using Library.Context.Perfil.Person;
using Library.Models.Perfil;
using System.Collections.Generic;

namespace api_perfil.Persistence
{
    public class PersonPersistence
    {
        private readonly ClassPerson classPerson;
        public PersonPersistence()
        {
            classPerson = new ClassPerson();
        }

        public IEnumerable<Person> List()
        {
            var listPersonApi = new List<Person>();
            var allPerson = classPerson.List();

            if (allPerson != null)
            {
                foreach (var people in allPerson)
                {
                    var person = new Person()
                    {
                        Id = people.Id,
                        FirstName = people.FirstName,
                        LastName = people.LastName,
                        Age = people.Age,
                        Birthday = people.Birthday,
                        CountryId = people.CountryId,
                        Picture = new Pictures()
                        {
                            Id = people.Picture.Id,
                            Symbol = people.Picture.Symbol,
                            Path = people.Picture.Path
                        },
                        Contacts = new Contacts()
                        {
                            Id = people.Contacts.Id,
                            Email = people.Contacts.Email,
                            Mobile = people.Contacts.Mobile
                        }
                    };

                    listPersonApi.Add(person);
                }

                return listPersonApi;
            }
            else
            {
                return null;
            }
        }
        public Person Get(int? Id)
        {
            var people = classPerson.Get(Id);

            if (people != null)
            {
                var person = new Person()
                {
                    Id = people.Id,
                    FirstName = people.FirstName,
                    LastName = people.LastName,
                    Birthday = people.Birthday,
                    Age = people.Age,
                    Picture = new Pictures()
                    {
                        Id = people.Picture.Id,
                        Symbol = people.Picture.Symbol,
                        Path = people.Picture.Path
                    },
                    Contacts = new Contacts()
                    {
                        Id = people.Contacts.Id,
                        Email = people.Contacts.Email,
                        Mobile = people.Contacts.Mobile
                    },
                    CountryId = people.CountryId

                };
                return person;
            }
            else
            {
                return null;
            }
        }
        public void Post(Person person)
        {
            var personDomain = new PersonDomain()
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Birthday = person.Birthday,
                Age = person.Age,
                CountryId = person.CountryId,
                Picture = new PicturesDomain()
                {
                    Id = person.Picture.Id,
                    Symbol = person.Picture.Symbol,
                    Path = person.Picture.Path
                },
                Contacts = new ContactsDomain()
                {
                    Id = person.Contacts.Id,
                    Email = person.Contacts.Email,
                    Mobile = person.Contacts.Mobile
                },
            };

            classPerson.Post(personDomain);
        }
        public void Put(Person person, int? Id)
        {
            var personDomain = new PersonDomain()
            {
                Id = person.Id,
                FirstName = person.FirstName,
                LastName = person.LastName,
                Birthday = person.Birthday,
                Age = person.Age,
                CountryId = person.CountryId,
                Picture = new PicturesDomain()
                {
                    Id = person.Picture.Id,
                    Symbol = person.Picture.Symbol,
                    Path = person.Picture.Path
                },
                Contacts = new ContactsDomain()
                {
                    Id = person.Contacts.Id,
                    Email = person.Contacts.Email,
                    Mobile = person.Contacts.Mobile
                },
            };

            classPerson.Put(personDomain, Id);
        }
        public void Delete(int? Id)
        {
            classPerson.Delete(Id);
        }
    }
}