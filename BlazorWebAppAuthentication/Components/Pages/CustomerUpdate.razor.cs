using BlazorWebAppAuthentication.Database;
using BlazorWebAppAuthentication.Domain;
using BlazorWebAppAuthentication.Domain.Entities;
using BlazorWebAppAuthentication.Domain.Services;
using BlazorWebAppAuthentication.Models.ViewModels;
using Microsoft.AspNetCore.Components;

namespace BlazorWebAppAuthentication.Components.Pages;

public partial class CustomerUpdate
{
    [Inject]
    public ICustomerService CustomerService { get; set; }
    [Inject]
    public UserAccountRepository UserAccountRepository { get; set; }
    [Inject]
    public ICountryService CountryService { get; set; }
    [Inject]
    public NavigationManager NavigationManager { get; set; }
        
    [Parameter]
    public string CustomerId { get; set; }
    
    public List<Country> Countries = new List<Country>();
    public List<MaritalStatus> MaritalStatusList { get; set; } = new List<MaritalStatus>();
    public List<Role> Roles { get; set; } = new List<Role>();
    
    protected string CountryId = string.Empty;
    
    
    //used to store state of screen
    protected string Message = string.Empty;
    protected string StatusClass = string.Empty;
    protected bool Saved;
    protected Guid TempPassword = Guid.Empty;


    public Customer Customer { get; set; } = new();

    protected override async Task OnInitializedAsync()
    {
            Saved = false;
            Countries =   CountryService.GetAllCountries();
            //Customer =   CustomerService.GetCustomerById(CustomerId);
            MaritalStatusList = Enum.GetValues(typeof(MaritalStatus)).Cast<MaritalStatus>().ToList();

            int.TryParse(CustomerId, out var customerId);

            if (customerId == 0)
            {
                Customer = new Customer { CountryId = 1, BirthDate = DateTime.Now, JoinedDate = DateTime.Now };
            }
            else
            {
                Customer =   CustomerService.GetCustomerById(int.Parse(CustomerId));
            }

            CountryId = Customer.CountryId.ToString();
        
    }
    public async Task OnValidSubmit()
    {
        Console.WriteLine("OnValidSubmit");
        
        Saved = false;
        Customer.CountryId = int.Parse(CountryId);

        if (Customer.CustomerId == 0)
        {
            var addedCustomer = CustomerService.AddCustomer(Customer);
            if (addedCustomer != null)
            {
                StatusClass = "alert-success";
                TempPassword = Guid.NewGuid();
                Message = $"New employee added successfully. In your first login please use below password. PASSWORD: {TempPassword}";
                Saved = true;
                var lastUserAccount = UserAccountRepository.GetLastUserAccount();
                var userAccount = new UserAccount()
                {
                    Email = Customer.Email,
                    Password = TempPassword.ToString(),
                    UserAccountId = lastUserAccount.UserAccountId + 1,
                    Role = Customer.Role,
                    Username = ""
                };
                UserAccountRepository.AddUserAccount(userAccount);
            }else
            {
                StatusClass = "alert-danger";
                Message = "Something went wrong adding the new employee. Please try again.";
                Saved = false;
            }
        }else
        {
            CustomerService.UpdateCustomer(Customer);
            StatusClass = "alert-success";
            Message = "Employee updated successfully.";
            Saved = true;
        }
    }
    public void OnInvalidSubmit()
    {
        Console.WriteLine("OnInvalidSubmit");
        StatusClass = "alert-danger";
        Message = "There are some validation errors. Please try again.";
    }
    protected async Task DeleteCustomer()
    {
        CustomerService.DeleteCustomer(Customer.CustomerId);

        StatusClass = "alert-success";
        Message = "Deleted successfully";

        Saved = true;
    }
    public string GetMaritalStatusName(MaritalStatus status)
    {
        return Enum.GetName(typeof(MaritalStatus), status);
    }
    protected void NavigateToOverview()
    {
        NavigationManager.NavigateTo("/customers-overview");
    }
    
}