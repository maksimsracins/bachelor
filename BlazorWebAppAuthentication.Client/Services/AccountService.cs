using BlazorWebAppAuthentication.Client.Services.Interfaces;
using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Client.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Account AddAccount(Account account)
        {
            return _accountRepository.AddAccount(account);
        }

        public Account GetAccountById(int accountId)
        {
            return _accountRepository.GetAccountById(accountId);
        }

        public Account UpdateAccount(Account account)
        {
            return _accountRepository.UpdateAccount(account);
        }

        public Account DeleteAccount(int accountId)
        {
            return _accountRepository.DeleteAccount(accountId);
        }

        public List<Account> GetAllAccounts()
        {
            return _accountRepository.GetAllAccounts();
        }
    }
}