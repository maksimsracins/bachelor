using BlazorWebAppAuthentication.Domain;

namespace BlazorWebAppAuthentication.Database;

public interface ICountryRepository
{
    IEnumerable<Country> GetAllCountries();
    Country GetCountryById(int countryId);
    void AddCountry(Country country);
    void UpdateCountry(Country country);
    void DeleteCountry(int countryId);
}