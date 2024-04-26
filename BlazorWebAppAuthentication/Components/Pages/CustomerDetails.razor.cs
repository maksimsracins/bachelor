using BlazorWebAppAuthentication.Database;
using BlazorWebAppAuthentication.Domain;
using BlazorWebAppAuthentication.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace BlazorWebAppAuthentication.Components.Pages;

public partial class CustomerDetails
{
    [Parameter]
    public int CustomerId { get; set; }
    [Inject]
    public ICountryRepository CountryRepository { get; set; }
    
    [Inject]
    public ICustomerRepository CustomerRepository { get; set; }
    public Customer? Customer { get; set; } = new Customer();
    public List<Country>? Country { get; set; } = new List<Country>();

    protected override async Task OnInitializedAsync()
    {
        Country = CountryRepository.GetAllCountries().ToList();
        Customer = CustomerRepository.GetCustomerById(CustomerId);
    }
}