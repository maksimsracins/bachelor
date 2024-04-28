using BlazorWebAppAuthentication.Domain;
using BlazorWebAppAuthentication.Domain.Entities;
using BlazorWebAppAuthentication.Domain.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorWebAppAuthentication.Components.Pages.Account;

public partial class Register
{
    [Inject] 
    private ICountryService CountryService { get; set; }
    
    public Customer CreateDefaultCustomer()
    {
        return new Customer
        {
            CustomerId = 0,
            FirstName = "Empty",
            LastName = "Empty",
            Accounts = new List<Domain.Entities.Account>(),
            Email = "",
            City = "Empty",
            Country = CountryService.GetCountryById(1),
            PhoneNumber = "9999999",
            CountryId = 1, 
            BirthDate = DateTime.Now, 
            JoinedDate = DateTime.Now,
            Street = "Empty",
            Zip = "Empty",
            Role = Role.User.ToString()
        };
    }
}