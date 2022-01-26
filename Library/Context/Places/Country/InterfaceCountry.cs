using Library.Models.Places;
using System.Collections.Generic;

namespace Library.Context.Places.Country
{
    public interface InterfaceCountry
    {
        IEnumerable<CountryDomain> List();
        CountryDomain Get(int? Id);
        void Post(CountryDomain countryDomain);
        void Put(CountryDomain countryDomain, int? Id);
        void Delete(int? Id);
    }
}
