using BlazorWebAppAuthentication.Components.Layout;
using BlazorWebAppAuthentication.Domain;
using BlazorWebAppAuthentication.Domain.Entities;
using BlazorWebAppAuthentication.Domain.Services;
using BlazorWebAppAuthentication.Models;
using BlazorWebAppAuthentication.Payment;
using Microsoft.AspNetCore.Components;

namespace BlazorWebAppAuthentication.Components.Pages;

public partial class TransferMoney
{
    [CascadingParameter]
    public HttpContext? HttpContext { get; set; }

    [SupplyParameterFromForm] 
    public TransferModel Model { get; set; } = new();
    
    [Inject]
    public CustomerService CustomerService { get; set; }
    
    [Inject]
    public NavMenu NavMenu { get; set; }
    
    [Inject]
    public PaymentService PaymentService { get; set; }
    public MT103Payment Mt103Payment { get; set; } = new MT103Payment();

    public decimal accountBalance = 0;
    
    private Domain.Entities.Account selectedSenderAccount;
    private bool showTransferForm = true;
    
    private string? transactionStatusMessage;
    public List<Domain.Entities.Account> senderAccounts { get; set; } = new List<Domain.Entities.Account>();

    protected override async Task OnInitializedAsync()
    { 
        senderAccounts = ApplicationContext.Accounts.Where(a => a.AccountId == 1).ToList();
    }
    
    private async Task HandleTransfer()
    {
        var senderUserAccount = NavMenu.GetUserAccount();
        var senderCustomer = CustomerService.GetAllCustomers()
            .FirstOrDefault(c => c.UserAccountId == senderUserAccount.UserAccountId);

        var beneficiaryCustomerId = ApplicationContext.Accounts
            .FirstOrDefault(a => a.AccountId == Model.BeneficiaryAccountId)
            .CustomerId;
        var beneficiaryCustomer = CustomerService.GetCustomerById(beneficiaryCustomerId);
        
        var senderAccount = ApplicationContext.Accounts
            .FirstOrDefault(a => a.AccountId == @Model.SenderAccountId);
        Model.SenderAccountName = senderAccount.AccountName;

        var beneficiaryAccount = ApplicationContext.Accounts
            .FirstOrDefault(a => a.AccountName == Model.BeneficiaryAccountName);

        Model.BeneficiaryAccountName = beneficiaryAccount.AccountName;
        
        if (senderAccount == null || beneficiaryAccount == null)
        {
            transactionStatusMessage = "One or both account IDs are invalid.";
            return;
        }

        if (senderAccount.Balance < @Model.Amount)
        {
            transactionStatusMessage = "Insufficient funds.";
            return;
        }

        // Process transaction
        senderAccount.Balance -= @Model.Amount;
        beneficiaryAccount.Balance += @Model.Amount;

        var mt103Payment = PaymentService.EnrichMT103Payment(senderCustomer, beneficiaryCustomer, Model);
        var generatedSWIFT = PaymentService.GenerateMT103TextFile(mt103Payment);
        Console.WriteLine(generatedSWIFT);

        var transaction = new Transaction
        {
            SenderAccountId = @Model.SenderAccountId,
            BeneficiaryAccountId = @Model.BeneficiaryAccountId,
            Amount = @Model.Amount,
            TransactionStatus = TransactionStatus.Processed, // Assuming instant completion
            TransactionType = "MT103",
            RemittanceInfo = @Model.RemittenceInfo
        };

        ApplicationContext.Transactions.Add(transaction);
          ApplicationContext.SaveChangesAsync();

        transactionStatusMessage = "Transaction completed successfully.";
        //navigationManager.NavigateTo("/transactionhistory");
    }
}