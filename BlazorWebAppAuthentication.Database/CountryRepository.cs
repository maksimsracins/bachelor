using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain;
using BlazorWebAppAuthentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebAppAuthentication.Database
{
    public class CountryRepository : ICountryRepository
    {
        private readonly ApplicationContext _context;

        public CountryRepository(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<Country> GetAllCountries()
        {
            return _context.Countries.ToList();
        }

        public Country GetCountryById(int countryId)
        {
            return _context.Countries.FirstOrDefault(c => c.CountryId == countryId);
        }

        public Country AddCountry(Country country)
        {
            var data = _context.Countries.Add(country);
              _context.SaveChangesAsync();
              return data.Entity;
        }

        public Country UpdateCountry(Country country)
        {
            var data= _context.Countries.Update(country);
              _context.SaveChangesAsync();
              return data.Entity;
        }

        public Country DeleteCountry(int countryId)
        {
            var data = GetCountryById(countryId); 
            var result =_context.Countries.Remove(data);
            _context.SaveChangesAsync();
            return result.Entity;

        }
    }
}