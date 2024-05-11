using BlazorWebAppAuthentication.Client.Services;
using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;
using Moq;

namespace BlazorWebAppAuthentication.Tests;

public class CountryServiceTests
{
    private CountryService _countryService;
    private Mock<ICountryRepository> _mockCountryRepository;

    [SetUp]
    public void Setup()
    {
        _mockCountryRepository = new Mock<ICountryRepository>();
        _countryService = new CountryService(_mockCountryRepository.Object);
    }
    [Test]
    public void GetAllCountries_ShouldReturnAllCountries()
    {
        // Arrange
        var countries = new List<Country> { new Country { CountryId = 1, Name = "USA" }, new Country { CountryId = 2, Name = "Canada" } };
        _mockCountryRepository.Setup(repo => repo.GetAllCountries()).Returns(countries);

        // Act
        var result = _countryService.GetAllCountries();

        // Assert
        Assert.AreEqual(2, result.Count);
        Assert.That(result, Is.EqualTo(countries));
        _mockCountryRepository.Verify(repo => repo.GetAllCountries(), Times.Once);
    }
    [Test]
    public void GetCountryById_ShouldReturnCountry()
    {
        // Arrange
        var country = new Country { CountryId = 1, Name = "USA" };
        _mockCountryRepository.Setup(repo => repo.GetCountryById(1)).Returns(country);

        // Act
        var result = _countryService.GetCountryById(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("USA", result.Name);
        _mockCountryRepository.Verify(repo => repo.GetCountryById(1), Times.Once);
    }
    [Test]
    public void AddCountry_ShouldReturnAddedCountry()
    {
        // Arrange
        var country = new Country { CountryId = 3, Name = "Mexico" };
        _mockCountryRepository.Setup(repo => repo.AddCountry(It.IsAny<Country>())).Returns(country);

        // Act
        var result = _countryService.AddCountry(country);

        // Assert
        Assert.AreEqual("Mexico", result.Name);
        _mockCountryRepository.Verify(repo => repo.AddCountry(country), Times.Once);
    }
    [Test]
    public void UpdateCountry_ShouldReturnUpdatedCountry()
    {
        // Arrange
        var country = new Country { CountryId = 1, Name = "USA" };
        _mockCountryRepository.Setup(repo => repo.UpdateCountry(It.IsAny<Country>())).Returns(country);

        // Act
        var result = _countryService.UpdateCountry(country);

        // Assert
        Assert.AreEqual("USA", result.Name);
        _mockCountryRepository.Verify(repo => repo.UpdateCountry(country), Times.Once);
    }
    [Test]
    public void DeleteCountry_ShouldReturnDeletedCountry()
    {
        // Arrange
        var country = new Country { CountryId = 1, Name = "USA" };
        _mockCountryRepository.Setup(repo => repo.GetCountryById(1)).Returns(country);
        _mockCountryRepository.Setup(repo => repo.DeleteCountry(1));

        // Act
        var result = _countryService.DeleteCountry(1);

        // Assert
        Assert.AreEqual("USA", result.Name);
        _mockCountryRepository.Verify(repo => repo.GetCountryById(1), Times.Once);
        _mockCountryRepository.Verify(repo => repo.DeleteCountry(1), Times.Once);
    }

}
