using BlazorWebAppAuthentications.API.Models;
using Microsoft.AspNetCore.Mvc;
using UserAccount = BlazorWebAppAuthentication.Domain.Entities.UserAccount;

namespace BlazorWebAppAuthentications.API.Controllers;

[Route("/api/[controller]")]
[ApiController]
public class UserAccountController : Controller
{
    private readonly IUserAccountsRepository _userAccountsRepository;

    public UserAccountController(IUserAccountsRepository userAccountsRepository)
    {
        _userAccountsRepository = userAccountsRepository;
    }

    [HttpGet]
    public IActionResult GetAllUserAccounts() => Ok(_userAccountsRepository.GetAllUserAccounts());

    [HttpPost]
    public IActionResult AddUserAccount(UserAccount userAccount)
    {
        return Ok(_userAccountsRepository.AddUserAccount(userAccount));
    }
}