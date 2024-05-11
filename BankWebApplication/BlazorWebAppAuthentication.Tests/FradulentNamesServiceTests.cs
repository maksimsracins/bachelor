using BlazorWebAppAuthentication.Client.Services;
using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;
using Moq;

namespace BlazorWebAppAuthentication.Tests;

public class FraudulentNamesServiceTests
{
    private FraudulentNamesService _fraudulentNamesService;
    private Mock<IFradulentNamesRepository> _mockFradulentNamesRepository;

    [SetUp]
    public void Setup()
    {
        _mockFradulentNamesRepository = new Mock<IFradulentNamesRepository>();
        _fraudulentNamesService = new FraudulentNamesService(_mockFradulentNamesRepository.Object);
    }
    [Test]
    public void GetFraudulentNamesById_ShouldReturnNames()
    {
        // Arrange
        var id = 1;
        var fraudulentNames = new List<FraudulentNames>
        {
            new FraudulentNames { Id = id, Name = "John Doe" }
        };
        _mockFradulentNamesRepository.Setup(x => x.GetFraudulentNamesById(id)).Returns(fraudulentNames);

        // Act
        var result = _fraudulentNamesService.GetFraudulentNamesById(id);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Count);
        Assert.AreEqual("John Doe", result[0].Name);
        _mockFradulentNamesRepository.Verify(x => x.GetFraudulentNamesById(id), Times.Once);
    }
    [Test]
    public void GetFraudulentName_ShouldReturnNameIfExists()
    {
        // Arrange
        var name = "John Doe";
        _mockFradulentNamesRepository.Setup(x => x.GetFraudulentName(name)).Returns(name);

        // Act
        var result = _fraudulentNamesService.GetFraudulentName(name);

        // Assert
        Assert.AreEqual(name, result);
        _mockFradulentNamesRepository.Verify(x => x.GetFraudulentName(name), Times.Once);
    }
    [Test]
    public void GetAllFraudulentNames_ShouldReturnAllNames()
    {
        // Arrange
        var fraudulentNames = new List<FraudulentNames>
        {
            new FraudulentNames { Id = 1, Name = "John Doe" },
            new FraudulentNames { Id = 2, Name = "Jane Smith" }
        };
        _mockFradulentNamesRepository.Setup(x => x.GetAllFraudulentNames()).Returns(fraudulentNames);

        // Act
        var result = _fraudulentNamesService.GetAllFraudulentNames();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        _mockFradulentNamesRepository.Verify(x => x.GetAllFraudulentNames(), Times.Once);
    }

}
