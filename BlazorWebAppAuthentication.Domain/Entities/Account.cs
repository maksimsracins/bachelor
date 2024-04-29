namespace BlazorWebAppAuthentication.Domain.Entities;

public class Account
{
    public int AccountId { get; set; }
    public string AccountName { get; set; }
    
    public decimal Balance { get; set; }
    public AccountType AccountType { get; set; }
    public int CustomerId { get; set; }

}