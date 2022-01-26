using api_places.Models.Places;
using Library.Context.Places.Country;
using Library.Models.Places;
using System.Collections.Generic;

namespace api_places.Persistence
{
    public class CountryPersistence
    {
        private readonly ClassCountry classCountry;

        public CountryPersistence()
        {
            classCountry = new ClassCountry();
        }

        public IEnumerable<Country> List()
        {
            var listCountries = new List<Country>();
            var allCountries = classCountry.List();

            if (allCountries != null)
            {
                foreach (var countries in allCountries)
                {
                    var flag = new Flag()
                    {
                        Id = countries.Flag.Id,
                        Symbol = countries.Flag.Symbol,
                        Path = countries.Flag.Path
                    };
                    var country = new Country()
                    {
                        Id = countries.Id,
                        Label = countries.Label,
                        Flag = flag
                    };
                    listCountries.Add(country);
                }

                return listCountries;
            }
            else
            {
                return null;
            }
        }
        public Country Get(int? Id)
        {
            var countries = classCountry.Get(Id);

            if (countries.Id.Equals(Id))
            {
                var flag = new Flag()
                {
                    Id = countries.Flag.Id,
                    Symbol = countries.Flag.Symbol,
                    Path = countries.Flag.Path
                };
                var country = new Country()
                {
                    Id = countries.Id,
                    Label = countries.Label,
                    Flag = flag
                };
                return country;
            }
            else
            {
                return null;
            }
        }
        public void Post(Country country)
        {
            var flagDomain = new FlagDomain()
            {
                Id = country.Flag.Id,
                Symbol = country.Flag.Symbol,
                Path = country.Flag.Path
            };
            var countryDomain = new CountryDomain()
            {
                Id = country.Id,
                Label = country.Label,
                Flag = flagDomain
            };

            classCountry.Post(countryDomain);
        }
        public void Put(Country country, int? Id)
        {
            var flagDomain = new FlagDomain()
            {
                Id = country.Flag.Id,
                Symbol = country.Flag.Symbol,
                Path = country.Flag.Path
            };
            var countryDomain = new CountryDomain()
            {
                Id = country.Id,
                Label = country.Label,
                Flag = flagDomain
            };

            classCountry.Put(countryDomain, Id);
        }
        public void Delete(int? Id)
        {
            classCountry.Delete(Id);
        }
    }
}