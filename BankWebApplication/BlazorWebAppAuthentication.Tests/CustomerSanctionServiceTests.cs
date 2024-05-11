using BlazorWebAppAuthentication.Client.Services;
using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;
using Moq;

namespace BlazorWebAppAuthentication.Tests;

public class CustomersSanctionStatusServiceTests
{
    private CustomersSanctionStatusService _service;
    private Mock<ICustomersSanctionStatusRepository> _mockRepository;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<ICustomersSanctionStatusRepository>();
        _service = new CustomersSanctionStatusService(_mockRepository.Object);
    }
    [Test]
    public void GetCustomerId_ShouldReturnCustomerId()
    {
        // Arrange
        var customerId = 1;
        _mockRepository.Setup(x => x.GetCustomerId(customerId)).Returns(customerId);

        // Act
        var result = _service.GetCustomerId(customerId);

        // Assert
        Assert.AreEqual(customerId, result);
        _mockRepository.Verify(x => x.GetCustomerId(customerId), Times.Once);
    }
    [Test]
    public void GetFraudulentNamesId_ShouldReturnFraudulentNamesId()
    {
        // Arrange
        var fraudulentNamesId = 1;
        _mockRepository.Setup(x => x.GetFraudulentNamesId(fraudulentNamesId)).Returns(fraudulentNamesId);

        // Act
        var result = _service.GetFraudulentNamesId(fraudulentNamesId);

        // Assert
        Assert.AreEqual(fraudulentNamesId, result);
        _mockRepository.Verify(x => x.GetFraudulentNamesId(fraudulentNamesId), Times.Once);
    }
    [Test]
    public void AddFraudulentCustomer_ShouldInvokeAddMethod()
    {
        // Arrange
        var customer = new CustomersSanctionStatus { Id = 1, CustomerId = 1, CustomerStatus = "Sanctioned" };
        _mockRepository.Setup(x => x.AddFraudulentCustomer(customer));

        // Act
        _service.AddFraudulentCustomer(customer);

        // Assert
        _mockRepository.Verify(x => x.AddFraudulentCustomer(customer), Times.Once);
    }


}
