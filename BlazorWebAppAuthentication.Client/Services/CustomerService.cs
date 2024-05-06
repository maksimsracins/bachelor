using BlazorWebAppAuthentication.Client.Services.Interfaces;
using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Client.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public IEnumerable<Customer> GetAllCustomers()
        {
            return   _customerRepository.GetAllCustomers();
        }

        public Customer AddCustomer(Customer? customer)
        {
            return _customerRepository.AddCustomer(customer);
        }

        public Customer GetCustomerById(int customerId)
        {
            return _customerRepository.GetCustomerById(customerId);
        }

        public Customer UpdateCustomer(Customer customer)
        {
              var data=_customerRepository.UpdateCustomer(customer);
              return data;
        }

        public Customer DeleteCustomer(int customerId)
        {
              var data =_customerRepository.DeleteCustomer(customerId);
              return data;
        }
    }
}