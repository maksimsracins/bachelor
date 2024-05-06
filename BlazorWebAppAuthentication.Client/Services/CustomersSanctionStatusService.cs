using BlazorWebAppAuthentication.Client.Services.Interfaces;
using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Client.Services;

public class CustomersSanctionStatusService : ICustomersSanctionStatusService
{
    private readonly ICustomersSanctionStatusRepository _customersSanctionStatusRepository;

    public CustomersSanctionStatusService(ICustomersSanctionStatusRepository customersSanctionStatusRepository)
    {
        _customersSanctionStatusRepository = customersSanctionStatusRepository;
    }
    
    public List<CustomersSanctionStatus> GetCustomersByStatus(string status)
    {
        return _customersSanctionStatusRepository.GetCustomersByStatus(status);
    }

    public int GetCustomerId(int id)
    {
        return _customersSanctionStatusRepository.GetCustomerId(id);
    }

    public int GetFraudulentNamesId(int id)
    {
        return _customersSanctionStatusRepository.GetFraudulentNamesId(id);
    }

    public List<CustomersSanctionStatus> GetAllCustomersSanctionStatusList()
    {
        throw new NotImplementedException();
    }
}