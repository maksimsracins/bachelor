using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentications.API.Models;

public class AccountsRepository : IAccountsRepository
{
    private readonly ApplicationContext _applicationContext;

    public AccountsRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }
    
    public Account GetAccountById(int id) => 
        _applicationContext.Accounts.FirstOrDefault(a => a.AccountId == id);


    public Account GetAccount(Account account) => 
        _applicationContext.Accounts.Find(account);

    public IEnumerable<Account> GetAccountsByCustomerId(int id) =>
        _applicationContext.Accounts.Where(a => a.CustomerId == id);

    public IEnumerable<Account> GetAllAccounts => _applicationContext.Accounts;

    public Account AddAccount(Account account)
    {
        var addedAccount = _applicationContext.Accounts.Add(account);
        _applicationContext.SaveChanges();
        return addedAccount.Entity;
    }

    public void DeleteAccount(int id)
    {
        var foundAccount = _applicationContext.Accounts.FirstOrDefault(a => a.AccountId == id);
        if (foundAccount == null) return;

        _applicationContext.Accounts.Remove(foundAccount);
        _applicationContext.SaveChanges();
    }

    public Account UpdateAccount(Account account)
    {
        var foundAccount = _applicationContext.Accounts.FirstOrDefault(a => a.AccountId == account.AccountId);

        if (foundAccount != null)
        {
            foundAccount.AccountId = account.AccountId;
            foundAccount.AccountType = account.AccountType;
            foundAccount.AccountName = account.AccountName;
            foundAccount.CustomerId = account.CustomerId;
            foundAccount.Balance = account.Balance;

            _applicationContext.SaveChanges();

            return foundAccount;
        }

        return null;
    }
}