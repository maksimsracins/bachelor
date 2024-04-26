using BlazorWebAppAuthentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebAppAuthentication.Components.Pages;

public partial class CustomerOverview
{
    private List<Customer>? Customers;

    protected override async Task OnInitializedAsync()
    {
        Customers = await ApplicationContext.Customers.ToListAsync();
    }

    private async Task ViewDetails(int customerId)
    {
        Console.WriteLine("Navigate to customer-details");
        NavigationManager.NavigateTo($"/customer-details/{customerId}");
    }
}