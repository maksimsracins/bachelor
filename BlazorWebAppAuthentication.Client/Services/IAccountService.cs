using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Client.Services
{
    public interface IAccountService
    {
        Account AddAccount(Account account);
        Account GetAccountById(int accountId);
        Account UpdateAccount(Account account);
        Account DeleteAccount(int accountId);
        List<Account> GetAllAccounts();
    }
}