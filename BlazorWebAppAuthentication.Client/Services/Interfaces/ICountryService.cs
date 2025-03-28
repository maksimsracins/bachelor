using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Client.Services.Interfaces;

public interface ICountryService
{
    List<Country> GetAllCountries();
    Country GetCountryById(int countryId);
    Country AddCountry(Country country);
    Country UpdateCountry(Country country);
    Country DeleteCountry(int countryId);
}