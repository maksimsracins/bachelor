using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Client.Services.Interfaces;

public interface ICustomerService
{
    IEnumerable<Customer> GetAllCustomers();
    Customer AddCustomer(Customer customer);
    Customer GetCustomerById(int customerId);
    Customer UpdateCustomer(Customer customer);
    Customer DeleteCustomer(int customerId);
}