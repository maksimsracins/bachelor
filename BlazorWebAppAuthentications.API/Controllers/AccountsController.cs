using BlazorWebAppAuthentication.Domain.Entities;
using BlazorWebAppAuthentications.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWebAppAuthentications.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AccountsController : Controller
{
    private readonly IAccountsRepository _accountsRepository;

    public AccountsController(IAccountsRepository accountsRepository)
    {
        _accountsRepository = accountsRepository;
    }

    [HttpGet]
    public IActionResult GetAccounts()
    {
        return Ok(_accountsRepository.GetAllAccounts);
    }

    [HttpGet("{id}")]
    public IActionResult GetAccountById(int id)
    {
        return Ok(_accountsRepository.GetAccountById(id));
    }

    [HttpDelete]
    public IActionResult DeleteAccount(int id)
    {
        if (id == 0) BadRequest();

        var accountToDelete = _accountsRepository.GetAccountById(id);
        if (accountToDelete == null) NotFound();
        
        _accountsRepository.DeleteAccount(id);

        return NoContent();
    }

    [HttpPut]
    public IActionResult UpdateAccount(Account account)
    {
        if (account == null) BadRequest();
        if (account.AccountName == string.Empty || account.AccountType == null) ModelState.AddModelError("Name/AccountName", "The name or account type should be present.");
        if (!ModelState.IsValid) BadRequest(ModelState);

        var accountToUpdate = _accountsRepository.GetAccountById(account.AccountId);
        if (accountToUpdate == null) BadRequest();

        _accountsRepository.UpdateAccount(account);

        return NoContent();
    }

    [HttpGet("{id}")]
    public IActionResult GetAccountsByCustomerId(int id)
    {
        return Ok(_accountsRepository.GetAccountsByCustomerId(id));
    }

    [HttpPost]
    public IActionResult AddAccount(Account account)
    {
        return Ok(_accountsRepository.AddAccount(account));
    }
}