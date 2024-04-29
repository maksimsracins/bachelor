namespace BlazorWebAppAuthentication.Domain.Entities;

public class Customer
{
    public int CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public DateTime BirthDate { get; set; }
    public string Street { get; set; }
    public string Zip { get; set; }
    public string City { get; set; }
    public int CountryId { get; set; }
    public Country? Country { get; set; } = default!;
    public string PhoneNumber { get; set; } = string.Empty;
    public MaritalStatus MaritalStatus { get; set; }
    public Gender Gender { get; set; }
    public List<Account>? Accounts { get; set; }
    public DateTime? JoinedDate { get; set; }
    public DateTime? ExitDate { get; set; }
    
    public int UserAccountId { get; set; }

}