using BlazorWebAppAuthentication.Domain;

namespace BlazorWebAppAuthentications.API.Models;

public interface ICountryRepository
{
    IEnumerable<Country> GetAllCountries();
    Country GetCountryById(int id);
}