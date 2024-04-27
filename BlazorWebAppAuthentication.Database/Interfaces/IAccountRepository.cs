using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Database.Interfaces;

public interface IAccountRepository
{
    Account AddAccount(Account account);
    Account GetAccountById(int accountId);
    Account UpdateAccount(Account account);
    Account DeleteAccount(int accountId);
}