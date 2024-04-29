using BlazorWebAppAuthentication.Client.Services;
using BlazorWebAppAuthentication.Domain.Entities;
using Microsoft.AspNetCore.Components;

namespace BlazorWebAppAuthentication.Client.Components.Pages;

public partial class CustomerOverview
{
    [Inject] 
    public ICustomerService CustomerService { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }
    
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