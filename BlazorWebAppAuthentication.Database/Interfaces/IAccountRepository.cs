using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Database;

public interface IAccountRepository
{
    void AddAccount(Account account);
    Account GetAccountById(int accountId);
    void UpdateAccount(Account account);
    void DeleteAccount(int accountId);
}