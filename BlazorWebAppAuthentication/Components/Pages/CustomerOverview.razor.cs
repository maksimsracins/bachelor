using BlazorWebAppAuthentication.Domain.Entities;
using BlazorWebAppAuthentication.Domain.Services;
using BlazorWebAppAuthentication.Services;
using Microsoft.AspNetCore.Components;

namespace BlazorWebAppAuthentication.Components.Pages;

public partial class CustomerOverview
{
    [Inject] 
    public ICustomerService CustomerService { get; set; }
    
    public IEnumerable<Customer> Customers { get; set; }

    protected override async Task OnInitializedAsync()
    {
        
        Customers = (  CustomerService.GetAllCustomers()).ToList();
    }

    private async Task ViewDetails(int customerId)
    {
        Console.WriteLine("Navigate to customer-details");
        NavigationManager.NavigateTo($"/customer-details/{customerId}");
    }
}