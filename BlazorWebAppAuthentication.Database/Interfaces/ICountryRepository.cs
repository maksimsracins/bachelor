using BlazorWebAppAuthentication.Domain;
using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Database.Interfaces;

public interface ICountryRepository
{
    IEnumerable<Country> GetAllCountries();
    Country GetCountryById(int countryId);
    Country AddCountry(Country country);
    Country UpdateCountry(Country country);
    Country DeleteCountry(int countryId);
}