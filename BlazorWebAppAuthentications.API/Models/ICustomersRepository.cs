using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentications.API.Models;

public interface ICustomersRepository
{

    Customer GetCustomerById(int id);
    IEnumerable<Customer> GetAllCustomers();
    Customer GetCustomer(Customer customer);
    Customer AddCustomer(Customer customer);
    void DeleteCustomer(int id);
    Customer UpdateCustomer(Customer customer);
}