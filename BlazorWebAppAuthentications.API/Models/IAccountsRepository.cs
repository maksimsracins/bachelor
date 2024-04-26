using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentications.API.Models;

public interface IAccountsRepository
{
    Account GetAccountById(int id);
    Account GetAccount(Account account);
    IEnumerable<Account> GetAccountsByCustomerId(int id);
    IEnumerable<Account> GetAllAccounts { get; }
    Account AddAccount(Account account);

    Account UpdateAccount(Account account);
    void DeleteAccount(int id);
}