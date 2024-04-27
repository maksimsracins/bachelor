using BlazorWebAppAuthentication.Domain;

namespace BlazorWebAppAuthentication.Database;

public interface ICountryRepository
{
    IEnumerable<Country> GetAllCountries();
    Country GetCountryById(int countryId);
    Country AddCountry(Country country);
    Country UpdateCountry(Country country);
    Country DeleteCountry(int countryId);
}