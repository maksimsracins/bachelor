using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Client.Services.Interfaces;

public interface ICustomersSanctionStatusService
{
    List<CustomersSanctionStatus> GetCustomersByStatus(string status);
    int GetCustomerId(int id);
    int GetFraudulentNamesId(int id);
    List<CustomersSanctionStatus> GetAllCustomersSanctionStatusList();
}