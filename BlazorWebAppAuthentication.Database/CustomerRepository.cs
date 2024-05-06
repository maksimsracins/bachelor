using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebAppAuthentication.Database
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationContext _context;

        public CustomerRepository(ApplicationContext context)
        {
            _context = context;
        }

        public  IEnumerable<Customer> GetAllCustomers()
        {
            return _context.Customers.ToList();
        }

        public Customer AddCustomer(Customer customer)
        {
            var data = _context.Customers.Add(customer);
            _context.SaveChanges();
            return data.Entity;
        }

        public Customer GetCustomerById(int customerId)
        {
            return _context.Customers
                .Include(c => c.Accounts)
                .FirstOrDefault(c => c.CustomerId == customerId);
        }

        public Customer UpdateCustomer(Customer? customer)
        {
            var data = _context.Customers.Update(customer);
            _context.SaveChangesAsync();
            return data.Entity;
        }

        public Customer DeleteCustomer(int customerId)
        {
            var data = _context.Customers.Find(customerId);
                var result = _context.Customers.Remove(data);
                _context.SaveChangesAsync();
                return result.Entity;
        }
    }
}