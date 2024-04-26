using BlazorWebAppAuthentication.Domain;

namespace BlazorWebAppAuthentication.Services;

public interface ICountryService
{
    IEnumerable<Country> GetAllCountries();
    Country GetCountryById(int countryId);
    void AddCountry(Country country);
    void UpdateCountry(Country country);
    void DeleteCountry(int countryId);
}