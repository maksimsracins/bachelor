using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Database;

public class CustomersSanctionStatusRepository : ICustomersSanctionStatusRepository
{
    
    private readonly ApplicationContext _context;

    public CustomersSanctionStatusRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public List<CustomersSanctionStatus> GetCustomersByStatus(string status)
    {
        return _context.CustomersSanctionStatus.Where(f => f.CustomerStatus == status)
            .ToList();
    }

    public int GetCustomerId(int id)
    {
        return _context.CustomersSanctionStatus.FirstOrDefault(f => f.CustomerId == id)!.CustomerId;
    }

    public int GetFraudulentNamesId(int id)
    {
        return _context.FraudulentNames.FirstOrDefault(f => f.Id == id)!.Id;
    }

    public List<CustomersSanctionStatus> GetAllCustomersSanctionStatusList()
    {
        return _context.CustomersSanctionStatus.ToList();
    }
}