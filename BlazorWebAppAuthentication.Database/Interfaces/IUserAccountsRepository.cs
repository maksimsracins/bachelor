using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Database.Interfaces;

public interface IUserAccountRepository
{
    UserAccount AddUserAccount(UserAccount userAccount);
    UserAccount GetUserAccountById(int userAccountId);
    void UpdateUserAccount(UserAccount userAccount);
    void DeleteUserAccount(int userAccountId);
    List<UserAccount> GetAllUserAccounts();
}