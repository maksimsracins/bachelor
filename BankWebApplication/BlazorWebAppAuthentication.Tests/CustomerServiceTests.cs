using BlazorWebAppAuthentication.Client.Services;
using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;
using Moq;

namespace BlazorWebAppAuthentication.Tests;

public class CustomerServiceTests
{
    private CustomerService _customerService;
    private Mock<ICustomerRepository> _mockCustomerRepository;

    [SetUp]
    public void Setup()
    {
        _mockCustomerRepository = new Mock<ICustomerRepository>();
        _customerService = new CustomerService(_mockCustomerRepository.Object);
    }
    [Test]
    public void GetAllCustomers_ShouldReturnAllCustomers()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { CustomerId = 1, FirstName = "John Doe" },
            new Customer { CustomerId = 2, FirstName = "Jane Smith" }
        };
        _mockCustomerRepository.Setup(repo => repo.GetAllCustomers()).Returns(customers.AsEnumerable());

        // Act
        var result = _customerService.GetAllCustomers();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count());
        _mockCustomerRepository.Verify(repo => repo.GetAllCustomers(), Times.Once);
    }
    [Test]
    public void AddCustomer_ShouldReturnAddedCustomer()
    {
        // Arrange
        var customer = new Customer { CustomerId = 3, FirstName = "Alice Johnson" };
        _mockCustomerRepository.Setup(repo => repo.AddCustomer(It.IsAny<Customer>())).Returns(customer);

        // Act
        var result = _customerService.AddCustomer(customer);

        // Assert
        Assert.AreEqual("Alice Johnson", result.FirstName);
        _mockCustomerRepository.Verify(repo => repo.AddCustomer(customer), Times.Once);
    }
    [Test]
    public void GetCustomerById_ShouldReturnCustomer()
    {
        // Arrange
        var customer = new Customer { CustomerId = 1, FirstName = "John Doe" };
        _mockCustomerRepository.Setup(repo => repo.GetCustomerById(1)).Returns(customer);

        // Act
        var result = _customerService.GetCustomerById(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("John Doe", result.FirstName);
        _mockCustomerRepository.Verify(repo => repo.GetCustomerById(1), Times.Once);
    }
    [Test]
    public void UpdateCustomer_ShouldReturnUpdatedCustomer()
    {
        // Arrange
        var customer = new Customer { CustomerId = 1, FirstName = "John Doe Updated" };
        _mockCustomerRepository.Setup(repo => repo.UpdateCustomer(It.IsAny<Customer>())).Returns(customer);

        // Act
        var result = _customerService.UpdateCustomer(customer);

        // Assert
        Assert.AreEqual("John Doe Updated", result.FirstName);
        _mockCustomerRepository.Verify(repo => repo.UpdateCustomer(customer), Times.Once);
    }
    [Test]
    public void DeleteCustomer_ShouldReturnDeletedCustomer()
    {
        // Arrange
        var customer = new Customer { CustomerId = 1, FirstName = "John Doe" };
        _mockCustomerRepository.Setup(repo => repo.DeleteCustomer(1)).Returns(customer);

        // Act
        var result = _customerService.DeleteCustomer(1);

        // Assert
        Assert.AreEqual("John Doe", result.FirstName);
        _mockCustomerRepository.Verify(repo => repo.DeleteCustomer(1), Times.Once);
    }

}
