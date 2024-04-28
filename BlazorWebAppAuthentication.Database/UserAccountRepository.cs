using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Database;

public class UserAccountRepository : IUserAccountRepository
{
    private readonly ApplicationContext _context;

    public UserAccountRepository(ApplicationContext context)
    {
        _context = context;
    }

    public void AddUserAccount(UserAccount userAccount)
    {
        _context.UserAccount.Add(userAccount);
        _context.SaveChanges();
    }

    public UserAccount GetLastUserAccount()
    {
        return _context.UserAccount.Last();
    }

    public UserAccount GetUserAccountById(int userAccountId)
    {
        return _context.UserAccount.FirstOrDefault(ua => ua.UserAccountId == userAccountId);
    }

    public void UpdateUserAccount(UserAccount userAccount)
    {
        _context.UserAccount.Update(userAccount);
        _context.SaveChanges();
    }

    public void DeleteUserAccount(int userAccountId)
    {
        var userAccount = _context.UserAccount.Find(userAccountId);
        if (userAccount != null)
        {
            _context.UserAccount.Remove(userAccount);
            _context.SaveChanges();
        }
    }
}