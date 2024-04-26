using BlazorWebAppAuthentication.Database;
using BlazorWebAppAuthentication.Domain;

namespace BlazorWebAppAuthentication.Services;

public class CountryService : ICountryService
{
    private readonly ICountryRepository _countryRepository;

    public CountryService(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    public IEnumerable<Country> GetAllCountries()
    {
        return _countryRepository.GetAllCountries();
    }

    public Country GetCountryById(int countryId)
    {
        return _countryRepository.GetCountryById(countryId);
    }

    public void AddCountry(Country country)
    {
        if (country == null)
        {
            throw new ArgumentNullException(nameof(country), "Country object cannot be null.");
        }
        _countryRepository.AddCountry(country);
    }

    public void UpdateCountry(Country country)
    {
        if (country == null)
        {
            throw new ArgumentNullException(nameof(country), "Country object cannot be null.");
        }
        _countryRepository.UpdateCountry(country);
    }

    public void DeleteCountry(int countryId)
    {
        var country = GetCountryById(countryId);
        if (country != null)
        {
            _countryRepository.DeleteCountry(countryId);
        }
        else
        {
            throw new ArgumentException($"Country with ID {countryId} not found.", nameof(countryId));
        }
    }
}