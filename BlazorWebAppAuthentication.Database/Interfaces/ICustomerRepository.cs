using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Database;

public interface ICustomerRepository
{
    IEnumerable<Customer> GetAllCustomers();
    void AddCustomer(Customer customer);
    Customer GetCustomerById(int customerId);
    void UpdateCustomer(Customer customer);
    void DeleteCustomer(int customerId);
}