using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Services;

public interface ICustomerService
{
    Task <IEnumerable<Customer>> GetAllCustomers();
    void AddCustomer(Customer customer);
    Customer GetCustomerById(int customerId);
    void UpdateCustomer(Customer customer);
    void DeleteCustomer(int customerId);
}