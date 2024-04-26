using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentications.API.Models;

public class UserAccountRepository : IUserAccountsRepository
{
    private protected ApplicationContext _applicationContext;

    public UserAccountRepository(ApplicationContext applicationContext)
    {
        _applicationContext = applicationContext;
    }

    public UserAccountRepository()
    {
    }

    public UserAccount GetUserAccount(UserAccount userAccount) =>
        _applicationContext.UserAccounts.Find(userAccount);

    public UserAccount AddUserAccount(UserAccount userAccount)
    {
        var addedUserAccount = _applicationContext.UserAccounts.Add(userAccount);
        _applicationContext.SaveChanges();
        return addedUserAccount.Entity;
    }

    public IEnumerable<UserAccount> GetAllUserAccounts() =>
        _applicationContext.UserAccounts;

    public IEnumerable<UserAccount> GetAllUserAccountsByName(string name) => 
        null;

    public IEnumerable<UserAccount> GetAllUserAccountsByRole(string role)
    {
        return null;
    }
}