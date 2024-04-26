using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentications.API.Models;

public class CustomersRepository : ICustomersRepository
{

    private readonly ApplicationContext _applicationContext;

    public CustomersRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public Customer GetCustomerById(int id) => _applicationContext.Customers.FirstOrDefault(c => c.CustomerId == id);

    public IEnumerable<Customer> GetAllCustomers() => _applicationContext.Customers;
    public Customer GetCustomer(Customer customer) => _applicationContext.Customers.Find(customer);

    public Customer AddCustomer(Customer customer)
    {
        var addedCustomer = _applicationContext.Customers.Add(customer);
        _applicationContext.SaveChanges();
        return addedCustomer.Entity;
    }

    public void DeleteCustomer(int id)
    {
        var foundCustomer = _applicationContext.Customers.FirstOrDefault(c => c.CustomerId == id);
        if (foundCustomer == null) return;

        _applicationContext.Customers.Remove(foundCustomer);
        _applicationContext.SaveChanges();
    }

    public Customer UpdateCustomer(Customer customer)
    {
        var foundCustomer = _applicationContext.Customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
        if (foundCustomer != null)
        {
            foundCustomer.CustomerId = customer.CustomerId;
            foundCustomer.FirstName = customer.FirstName;
            foundCustomer.LastName = customer.LastName;
            foundCustomer.BirthDate = customer.BirthDate;
            foundCustomer.Street = customer.Street;
            foundCustomer.Zip = customer.Zip;
            foundCustomer.City = customer.City;
            foundCustomer.CountryId = customer.CountryId;
            foundCustomer.Country = customer.Country;
            foundCustomer.PhoneNumber = customer.PhoneNumber;
            foundCustomer.MaritalStatus = customer.MaritalStatus;
            foundCustomer.Gender = customer.Gender;
            foundCustomer.Accounts = customer.Accounts;
            foundCustomer.JoinedDate = customer.JoinedDate;
            foundCustomer.ExitDate = customer.ExitDate;

            _applicationContext.SaveChanges();

            return foundCustomer;
        }
        return null;
    }
}