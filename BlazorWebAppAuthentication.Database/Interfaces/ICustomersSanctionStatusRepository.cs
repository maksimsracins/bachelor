using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Database.Interfaces;

public interface ICustomersSanctionStatusRepository
{
    List<CustomersSanctionStatus> GetCustomersByStatus(string status);
    int GetCustomerId(int id);
    int GetFraudulentNamesId(int id);
    List<CustomersSanctionStatus> GetAllCustomersSanctionStatusList();

}