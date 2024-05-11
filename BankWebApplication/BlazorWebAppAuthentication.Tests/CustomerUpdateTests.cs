using NUnit.Framework;
using Bunit;
using Moq;
using BlazorWebAppAuthentication.Client.Services.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;
using BlazorWebAppAuthentication.Client.Components.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;

namespace BlazorWebAppAuthentication.Tests
{
    public class CustomerUpdateTests
    {
        private Bunit.TestContext _context;

        [SetUp]
        public void Setup()
        {
            _context = new Bunit.TestContext();

            // Mocking services
            var mockCustomerService = new Mock<ICustomerService>();
            var mockCountryService = new Mock<ICountryService>();
            var mockNavigationManager = new Mock<NavigationManager>();

            // Setup Mocks to return data
            mockCountryService.Setup(service => service.GetAllCountries()).Returns(new List<Country> {
                new Country { CountryId = 1, Name = "United States" },
                new Country { CountryId = 2, Name = "Canada" }
            });

            mockCustomerService.Setup(service => service.GetCustomerById(It.IsAny<int>())).Returns(new Customer {
                CustomerId = 1,
                FirstName = "John",
                LastName = "Doe",
                CountryId = 1
            });

            // Registering services with bUnit's dependency injection container
            _context.Services.AddSingleton(mockCustomerService.Object);
            _context.Services.AddSingleton(mockCountryService.Object);
            _context.Services.AddSingleton(mockNavigationManager.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose(); // Clean up the test context to ensure no side effects
        }

    }
}
