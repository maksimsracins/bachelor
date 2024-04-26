using BlazorWebAppAuthentication.Domain;

namespace BlazorWebAppAuthentications.API.Models;

public class CountryRepository : ICountryRepository
{
    private protected ApplicationContext _applicationContext;

    public CountryRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public IEnumerable<Country> GetAllCountries() => _applicationContext.Countries;

    public Country GetCountryById(int id) => _applicationContext.Countries.FirstOrDefault(c => c.CountryId == id);
}