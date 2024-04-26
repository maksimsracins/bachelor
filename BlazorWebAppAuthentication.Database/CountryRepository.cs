using BlazorWebAppAuthentication.Domain;

namespace BlazorWebAppAuthentication.Database;

public class CountryRepository : ICountryRepository
{
    private readonly ApplicationContext _context;

    public CountryRepository(ApplicationContext context)
    {
        _context = context;
    }

    public IEnumerable<Country> GetAllCountries()
    {
        return _context.Countries;
    }

    public Country GetCountryById(int countryId)
    {
        return _context.Countries.FirstOrDefault(c => c.CountryId == countryId);
    }

    public void AddCountry(Country country)
    {
        _context.Countries.Add(country);
        _context.SaveChanges();
    }

    public void UpdateCountry(Country country)
    {
        _context.Countries.Update(country);
        _context.SaveChanges();
    }

    public void DeleteCountry(int countryId)
    {
        var country = GetCountryById(countryId);
        if (country != null)
        {
            _context.Countries.Remove(country);
            _context.SaveChanges();
        }
    }
}