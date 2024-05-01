using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Database;

public class FraudulentNamesRepository : IFradulentNamesRepository
{
    private readonly ApplicationContext _context;

    public FraudulentNamesRepository(ApplicationContext context)
    {
        _context = context;
    }
    public string GetFraudulentName(string name)
    {
        return _context.FraudulentNames.FirstOrDefault(fn => fn.Name == name)!.Name;
    }
    public List<FraudulentNames> GetFraudulentNamesById(int id)
    {
        return _context.FraudulentNames.Where(f => f.Id == id).ToList();
    }
    public List<FraudulentNames> GetAllFraudulentNames()
    {
        return _context.FraudulentNames.ToList();
    }
}