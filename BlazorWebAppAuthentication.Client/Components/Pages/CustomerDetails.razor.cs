using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace BlazorWebAppAuthentication.Client.Components.Pages;

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
        Country = (List<Country>?)  CountryRepository.GetAllCountries();
        Customer =   CustomerRepository.GetCustomerById(CustomerId);
    }
}