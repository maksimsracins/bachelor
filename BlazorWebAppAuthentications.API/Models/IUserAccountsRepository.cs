using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentications.API.Models;

public interface IUserAccountsRepository
{
    UserAccount GetUserAccount(UserAccount userAccount);

    IEnumerable<UserAccount> GetAllUserAccounts();
    IEnumerable<UserAccount> GetAllUserAccountsByName(string name);
    IEnumerable<UserAccount> GetAllUserAccountsByRole(string role);

    UserAccount AddUserAccount(UserAccount userAccount);
}