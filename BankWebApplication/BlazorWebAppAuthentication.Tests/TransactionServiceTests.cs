using BlazorWebAppAuthentication.Client.Services;
using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;
using Moq;

namespace BlazorWebAppAuthentication.Tests;

public class TransactionServiceTests
{
    private TransactionService _transactionService;
    private Mock<ITransactionRepository> _mockTransactionRepository;

    [SetUp]
    public void Setup()
    {
        _mockTransactionRepository = new Mock<ITransactionRepository>();
        _transactionService = new TransactionService(_mockTransactionRepository.Object);
    }
    [Test]
    public void AddTransaction_ShouldReturnAddedTransaction()
    {
        // Arrange
        var transaction = new Transaction { TransactionId = 1, Amount = 100.00m };
        _mockTransactionRepository.Setup(x => x.AddTransaction(It.IsAny<Transaction>())).Returns(transaction);

        // Act
        var result = _transactionService.AddTransaction(transaction);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(transaction.TransactionId, result.TransactionId);
        _mockTransactionRepository.Verify(x => x.AddTransaction(transaction), Times.Once);
    }
    [Test]
    public void GetTransactionById_ShouldReturnTransaction()
    {
        // Arrange
        var transaction = new Transaction { TransactionId = 1, Amount = 100.00m };
        _mockTransactionRepository.Setup(x => x.GetTransactionById(1)).Returns(transaction);

        // Act
        var result = _transactionService.GetTransactionById(1);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(transaction.TransactionId, result.TransactionId);
        _mockTransactionRepository.Verify(x => x.GetTransactionById(1), Times.Once);
    }
    [Test]
    public void UpdateTransaction_ShouldReturnUpdatedTransaction()
    {
        // Arrange
        var transaction = new Transaction { TransactionId = 1, Amount = 200.00m };
        _mockTransactionRepository.Setup(x => x.UpdateTransaction(It.IsAny<Transaction>())).Returns(transaction);

        // Act
        var result = _transactionService.UpdateTransaction(transaction);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(transaction.Amount, result.Amount);
        _mockTransactionRepository.Verify(x => x.UpdateTransaction(transaction), Times.Once);
    }
    [Test]
    public void DeleteTransaction_ShouldReturnDeletedTransaction()
    {
        // Arrange
        var transaction = new Transaction { TransactionId = 1, Amount = 100.00m };
        _mockTransactionRepository.Setup(x => x.DeleteTransaction(1)).Returns(transaction);

        // Act
        var result = _transactionService.DeleteTransaction(1);

        // Assert
        Assert.AreEqual(transaction.TransactionId, result.TransactionId);
        _mockTransactionRepository.Verify(x => x.DeleteTransaction(1), Times.Once);
    }
    [Test]
    public void GetAllTransactions_ShouldReturnAllTransactions()
    {
        // Arrange
        var transactions = new List<Transaction>
        {
            new Transaction { TransactionId = 1, Amount = 100.00m },
            new Transaction { TransactionId = 2, Amount = 200.00m }
        };
        _mockTransactionRepository.Setup(x => x.GetAllTransactions()).Returns(transactions);

        // Act
        var result = _transactionService.GetAllTransactions();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);
        _mockTransactionRepository.Verify(x => x.GetAllTransactions(), Times.Once);
    }

}
