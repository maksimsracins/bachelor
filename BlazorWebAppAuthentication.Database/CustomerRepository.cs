using BlazorWebAppAuthentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebAppAuthentication.Database;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationContext _context;

    public CustomerRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public IEnumerable<Customer> GetAllCustomers()
    {
        return _context.Customers;
    }

    public void AddCustomer(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
    }

    public Customer GetCustomerById(int customerId)
    {
        return _context.Customers.Include(c => c.Accounts).FirstOrDefault(c => c.CustomerId == customerId);
    }

    public void UpdateCustomer(Customer customer)
    {
        _context.Customers.Update(customer);
        _context.SaveChanges();
    }

    public void DeleteCustomer(int customerId)
    {
        var customer = _context.Customers.Find(customerId);
        if (customer != null)
        {
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }
    }
}