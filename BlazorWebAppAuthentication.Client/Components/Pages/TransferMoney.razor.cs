using System.Text;
using BlazorWebAppAuthentication.Client.Models.ViewModels;
using BlazorWebAppAuthentication.Client.Payment;
using BlazorWebAppAuthentication.Client.Services;
using BlazorWebAppAuthentication.Database;
using BlazorWebAppAuthentication.Domain.Entities;
using BlazorWebAppAuthentication.Domain.Enum;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace BlazorWebAppAuthentication.Client.Components.Pages;

public partial class TransferMoney
{
    [Inject]
    public required IHttpContextAccessor HttpContextAccessor { get; set; }

    [SupplyParameterFromForm] 
    public TransferModel Model { get; set; } = new();
    
    [Inject]
    public ICustomerService CustomerService { get; set; }
    
    [Inject]
    public IAccountService AccountService { get; set; }
    
    [Inject]
    public UserAccountRepository UserAccountRepository { get; set; }
    
    [Inject]
    public ITransactionService TransactionService { get; set; }
    
    [Inject]
    public PaymentService PaymentService { get; set; }
    
    [Inject]
    public IJSRuntime JSRuntime { get; set; }
    
    public MT103Payment Mt103Payment { get; set; } = new MT103Payment();
    public UserAccount loggedInUserAccount { get; set; }

    public decimal accountBalance = 0;
    private bool receiveCopy = false;
    
    private Account selectedSenderAccount;
    private bool showTransferForm = true;
    
    private string? transactionStatusMessage;
    public List<Account> senderAccounts { get; set; } = new List<Account>();

    protected override async Task OnInitializedAsync()
    {
        loggedInUserAccount = GetUserAccount();
        var customer = CustomerService.GetAllCustomers()
            .FirstOrDefault(c => c.UserAccountId == loggedInUserAccount.UserAccountId);
        
        
        senderAccounts = ApplicationContext.Accounts.Where(a => a.CustomerId == customer.CustomerId).ToList();
    }

    private async Task ConfirmTransfer()
    {
        var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", "Do you really want to send the payment?");
        if (confirmed)
        {
            await HandleTransfer();
        }
        else
        {
            transactionStatusMessage = "Transfer cancelled by user.";
        }
    }
    
    private async Task HandleTransfer()
    {
        bool isPaymentSuccessful;
        
        if (string.IsNullOrEmpty(Model.SenderAccountName) || string.IsNullOrEmpty(Model.BeneficiaryAccountName) )
        {
            isPaymentSuccessful = false;
            transactionStatusMessage = "Transfer cancelled by user.";
        }
        else
        {
            var senderCustomer = CustomerService.GetAllCustomers()
                .FirstOrDefault(c => c.UserAccountId == loggedInUserAccount.UserAccountId);
            var senderAccount = ApplicationContext.Accounts
                .FirstOrDefault(a => a.AccountName == Model.SenderAccountName);
            var beneficiaryCustomerId = ApplicationContext.Accounts
                .FirstOrDefault(a => a.AccountName == Model.BeneficiaryAccountName)!
                .CustomerId;
            var beneficiaryCustomer = CustomerService.GetCustomerById(beneficiaryCustomerId);
            var beneficiaryAccount = ApplicationContext.Accounts
                .FirstOrDefault(a => a.AccountName == Model.BeneficiaryAccountName);

            Model.SenderAccountId = senderAccount.AccountId;
            Model.SenderAccountName = senderAccount.AccountName;
            Model.BeneficiaryAccountId = beneficiaryAccount.AccountId.ToString();
            
            if (senderAccount == null || beneficiaryAccount == null)
            {
                isPaymentSuccessful = false;
                transactionStatusMessage = "One or both account IDs are invalid.";
                return;
            } else if (senderAccount.Balance < @Model.Amount)
            {
                isPaymentSuccessful = false;
                transactionStatusMessage = "Insufficient funds.";
                return;
            }else if (Model.Amount <= 0)
            {
                transactionStatusMessage = "Amount cannot be 0 or less.";
            }
            else
            {
                // Process transaction
                senderAccount.Balance -= @Model.Amount;
                beneficiaryAccount.Balance += @Model.Amount;
                var transaction = new Transaction
                {
                    SenderId = senderCustomer.CustomerId,
                    SenderAccountId = @Model.SenderAccountId,
                    BeneficiaryAccountId = @Model.BeneficiaryAccountId,
                    BeneficiaryId = beneficiaryCustomerId,
                    Amount = @Model.Amount,
                    TransactionStatus = TransactionStatus.Processed, // Assuming instant completion
                    TransactionType = @Model.TransactionType,
                    RemittanceInfo = @Model.RemittenceInfo
                };
                TransactionService.AddTransaction(transaction);
                transactionStatusMessage = "Transaction completed successfully.";
                if (senderCustomer != null){
                    if (receiveCopy)
                        {
                            if (Model.TransactionType == TransactionType.SWIFT)
                            {
                                await DonwloadMT103File(senderCustomer, beneficiaryCustomer);
                            }
                            else
                            {
                                await DonwloadPacs008File(senderCustomer, beneficiaryCustomer);
                            }
                        }
                }
            }
        }
        //navigationManager.NavigateTo("/transactionhistory");
    }
    public UserAccount GetUserAccount()
    {
        string? loggedUserEmail = HttpContextAccessor.HttpContext.User.Claims.FirstOrDefault().Value;

        var userAccount = UserAccountRepository
            .GetAllUserAccounts()
            .FirstOrDefault(ua => ua.Email == loggedUserEmail);

        return userAccount;
    }

    private async Task DonwloadPacs008File(Customer sender, Customer beneficiary)
    {
        var pacs008Payment = PaymentService.EnrichPacs008Payment(sender, beneficiary, Model);
        var generatedISO = PaymentService.GeneratePacs008Xml(pacs008Payment);

        var filename = $"Pacs008_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt";
        await JSRuntime.InvokeVoidAsync("downloadPayment", filename, "text/plain",
            Convert.ToBase64String(Encoding.UTF8.GetBytes(generatedISO)));
    }

    private async Task DonwloadMT103File(Customer sender, Customer beneficiary )
    {
        var mt103Payment = PaymentService.EnrichMT103Payment(sender, beneficiary, Model);
        var generatedSWIFT = PaymentService.GenerateMT103TextFile(mt103Payment);
        
        var filename = $"MT103_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt";
        await JSRuntime.InvokeVoidAsync("downloadPayment", filename, "text/plain",
            Convert.ToBase64String(Encoding.UTF8.GetBytes(generatedSWIFT)));
    }
}