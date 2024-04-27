using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Database.Interfaces;

public interface ICustomerRepository
{
    IEnumerable<Customer> GetAllCustomers();
    Customer AddCustomer(Customer customer);
    Customer GetCustomerById(int customerId);
    Customer UpdateCustomer(Customer? customer);
    Customer DeleteCustomer(int customerId);
}