using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Domain.Services
{
    public interface IAccountService
    {
        Account AddAccount(Account account);
        Account GetAccountById(int accountId);
        Account UpdateAccount(Account account);
        Account DeleteAccount(int accountId);
    }
}