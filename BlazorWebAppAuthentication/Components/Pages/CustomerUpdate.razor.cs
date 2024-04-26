using BlazorWebAppAuthentication.Domain;
using BlazorWebAppAuthentication.Domain.Entities;
using BlazorWebAppAuthentication.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorWebAppAuthentication.Components.Pages;

public partial class CustomerUpdate
{
    
    [Inject]
    public ICustomerService CustomerService { get; set; }
    
    [Inject]
    public ICountryService CountryService { get; set; }


    private IEnumerable<Country> Countries = new List<Country>();

    public List<MaritalStatus> MaritalStatusList { get; set; } = new List<MaritalStatus>();
    
    [Parameter]
    public int CustomerId { get; set; }

    public Customer Customer { get; set; } = new();
    

    protected override async void OnInitialized()
    {
        Countries = CountryService.GetAllCountries();
        Customer = CustomerService.GetCustomerById(CustomerId);
        MaritalStatusList = Enum.GetValues(typeof(MaritalStatus)).Cast<MaritalStatus>().ToList();
    }
    
    public static string GetMaritalStatusName(MaritalStatus status)
    {
        return Enum.GetName(typeof(MaritalStatus), status);
    }

    
}