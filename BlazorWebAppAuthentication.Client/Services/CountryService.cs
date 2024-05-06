using BlazorWebAppAuthentication.Client.Services.Interfaces;
using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Client.Services
{
    public class CountryService : ICountryService
    {
        private readonly ICountryRepository _countryRepository;

        public CountryService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public List<Country> GetAllCountries()
        {
            return _countryRepository.GetAllCountries().ToList();
        }

        public Country GetCountryById(int countryId)
        {
            return _countryRepository.GetCountryById(countryId);
        }

        public Country AddCountry(Country country)
        {
              return _countryRepository.AddCountry(country);
        }

        public Country UpdateCountry(Country country)
        {
             return _countryRepository.UpdateCountry(country);
        }

        public Country DeleteCountry(int countryId)
        {
            var country = GetCountryById(countryId);
            if (country != null)
            {
                  _countryRepository.DeleteCountry(countryId);
            }

            return country;
        }
    }
}