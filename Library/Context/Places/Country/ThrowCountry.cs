using Library.Models.Places;
using System;
using System.Collections.Generic;

namespace Library.Context.Places.Country
{
    public class ThrowCountry : InterfaceCountry
    {
        public IEnumerable<CountryDomain> List()
        {
            throw new NotImplementedException();
        }
        public CountryDomain Get(int? id)
        {
            throw new NotImplementedException();
        }
        public void Post(CountryDomain countryDomain)
        {
            throw new NotImplementedException();
        }
        public void Put(CountryDomain countryDomain, int? id)
        {
            throw new NotImplementedException();
        }
        public void Delete(int? id)
        {
            throw new NotImplementedException();
        }
    }
}
