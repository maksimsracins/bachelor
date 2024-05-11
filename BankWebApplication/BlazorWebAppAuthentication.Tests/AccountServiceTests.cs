using BlazorWebAppAuthentication.Client.Services;
using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;
using Moq;

namespace BlazorWebAppAuthentication.Tests;

public class AccountServiceTests
{
    private AccountService _accountService;
    private Mock<IAccountRepository> _mockAccountRepository;

    [SetUp]
    public void Setup()
    {
        _mockAccountRepository = new Mock<IAccountRepository>();
        _accountService = new AccountService(_mockAccountRepository.Object);
    }
    [Test]
    public void AddAccount_ShouldReturnAddedAccount()
    {
        // Arrange
        var account = new Account { AccountId = 1, AccountName = "Checking" };
        _mockAccountRepository.Setup(repo => repo.AddAccount(It.IsAny<Account>())).Returns(account);

        // Act
        var result = _accountService.AddAccount(account);

        // Assert
        Assert.AreEqual(account.AccountId, result.AccountId);
        _mockAccountRepository.Verify(repo => repo.AddAccount(account), Times.Once);
    }
    [Test]
    public void GetAccountById_ShouldReturnAccount()
    {
        // Arrange
        var account = new Account { AccountId = 1, AccountName = "Checking" };
        _mockAccountRepository.Setup(repo => repo.GetAccountById(It.IsAny<int>())).Returns(account);

        // Act
        var result = _accountService.GetAccountById(1);

        // Assert
        Assert.AreEqual(1, result.AccountId);
        _mockAccountRepository.Verify(repo => repo.GetAccountById(1), Times.Once);
    }
    [Test]
    public void UpdateAccount_ShouldReturnUpdatedAccount()
    {
        // Arrange
        var account = new Account { AccountId = 1, AccountName = "Savings" };
        _mockAccountRepository.Setup(repo => repo.UpdateAccount(It.IsAny<Account>())).Returns(account);

        // Act
        var result = _accountService.UpdateAccount(account);

        // Assert
        Assert.AreEqual("Savings", result.AccountName);
        _mockAccountRepository.Verify(repo => repo.UpdateAccount(account), Times.Once);
    }
    [Test]
    public void DeleteAccount_ShouldReturnDeletedAccount()
    {
        // Arrange
        var account = new Account { AccountId = 1, AccountName = "Checking" };
        _mockAccountRepository.Setup(repo => repo.DeleteAccount(It.IsAny<int>())).Returns(account);

        // Act
        var result = _accountService.DeleteAccount(1);

        // Assert
        Assert.AreEqual(1, result.AccountId);
        _mockAccountRepository.Verify(repo => repo.DeleteAccount(1), Times.Once);
    }
    [Test]
    public void GetAllAccounts_ShouldReturnAllAccounts()
    {
        // Arrange
        var accounts = new List<Account> { new Account { AccountId = 1, AccountName = "Checking" }, new Account { AccountId = 2, AccountName = "Savings" } };
        _mockAccountRepository.Setup(repo => repo.GetAllAccounts()).Returns(accounts);

        // Act
        var result = _accountService.GetAllAccounts();

        // Assert
        Assert.AreEqual(2, result.Count);
        _mockAccountRepository.Verify(repo => repo.GetAllAccounts(), Times.Once);
    }

}
