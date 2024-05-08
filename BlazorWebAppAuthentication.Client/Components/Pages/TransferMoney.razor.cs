using System.Text;
using BlazorWebAppAuthentication.Client.FraudPrevention;
using BlazorWebAppAuthentication.Client.Models.ViewModels;
using BlazorWebAppAuthentication.Client.Payment;
using BlazorWebAppAuthentication.Client.Services;
using BlazorWebAppAuthentication.Client.Services.Interfaces;
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
    public FraudPreventionService FraudPreventionService { get; set; }
    
    [Inject]
    public ICustomersSanctionStatusService CustomersSanctionStatusService { get; set; }
    
    [Inject]
    public IJSRuntime JSRuntime { get; set; }
    
    public MT103Payment Mt103Payment { get; set; } = new MT103Payment();
    public UserAccount loggedInUserAccount { get; set; }

    public decimal accountBalance = 0;
    private bool receiveCopy = false;
    
    private Account selectedSenderAccount;
    private bool showTransferForm = true;
    
    public bool isPaymentFraudulent { get; set; }
    
    private string? transactionStatusMessage;
    public List<Account> senderAccounts { get; set; } = new List<Account>();

    protected override async Task OnInitializedAsync()
    {
        loggedInUserAccount = GetUserAccount();
        var customer = CustomerService.GetAllCustomers()
            .FirstOrDefault(c => c.UserAccountId == loggedInUserAccount.UserAccountId);
        
        
        senderAccounts = ApplicationContext.Accounts.Where(a => a.CustomerId == customer.CustomerId).ToList();
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
                @Model.RemittenceInfo =
                    (string.IsNullOrEmpty(@Model.RemittenceInfo) ? "Transaction" : @Model.RemittenceInfo);
                
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

                switch (transaction.TransactionType)
                {
                    case TransactionType.ISO:
                        switch (senderAccount.AccountSupportType) 
                        {
                            case AccountSupportType.MTMX:
                                if (beneficiaryAccount.AccountSupportType == AccountSupportType.MT)
                                {
                                    var isFraudulent = await DonwloadConvertedToMT103File(senderCustomer, beneficiaryCustomer);
                                    if (!isFraudulent)
                                    {
                                                                            senderAccount.Balance -= @Model.Amount;
                                    beneficiaryAccount.Balance += @Model.Amount;
                                    TransactionService.AddTransaction(transaction);
                                    transactionStatusMessage = "Transaction completed successfully.";
                                    }
                                } else if (beneficiaryAccount.AccountSupportType == AccountSupportType.MX)
                                {
                                    var isFraudulent = await DonwloadPacs008File(senderCustomer, beneficiaryCustomer);
                                    if (!isFraudulent)
                                    {
                                        senderAccount.Balance -= @Model.Amount;
                                        beneficiaryAccount.Balance += @Model.Amount;
                                        TransactionService.AddTransaction(transaction);
                                        transactionStatusMessage = "Transaction completed successfully.";
                                    }
                                }
                                else 
                                {
                                    var isFraudulent = await DonwloadPacs008File(senderCustomer, beneficiaryCustomer);
                                    if (!isFraudulent)
                                    {
                                        senderAccount.Balance -= @Model.Amount;
                                        beneficiaryAccount.Balance += @Model.Amount;
                                        TransactionService.AddTransaction(transaction);
                                        transactionStatusMessage = "Transaction completed successfully.";
                                    }
                                }
                                break;
                            case AccountSupportType.MT:
                                if (beneficiaryAccount.AccountSupportType == AccountSupportType.MX)
                                {
                                    var isFraudulent = await DownloadConvertedToPacs008File(senderCustomer, beneficiaryCustomer);
                                    if (!isFraudulent)
                                    {
                                        senderAccount.Balance -= @Model.Amount;
                                        beneficiaryAccount.Balance += @Model.Amount;
                                        TransactionService.AddTransaction(transaction);
                                        transactionStatusMessage = "Transaction completed successfully.";
                                    }
                                } else if (beneficiaryAccount.AccountSupportType == AccountSupportType.MTMX)
                                {
                                    var isFraudulent = await DownloadConvertedToPacs008File(senderCustomer, beneficiaryCustomer);
                                    if (!isFraudulent)
                                    {
                                        senderAccount.Balance -= @Model.Amount;
                                        beneficiaryAccount.Balance += @Model.Amount;
                                        TransactionService.AddTransaction(transaction);
                                        transactionStatusMessage = "Transaction completed successfully.";
                                    }
                                }
                                else
                                {
                                    transactionStatusMessage = "Transaction rejected.";
                                }
                                break;
                            case AccountSupportType.MX:
                                if (beneficiaryAccount.AccountSupportType == AccountSupportType.MT)
                                {
                                    var isFraudulent = await DonwloadConvertedToMT103File(senderCustomer, beneficiaryCustomer);
                                    if (!isFraudulent)
                                    {
                                        senderAccount.Balance -= @Model.Amount;
                                        beneficiaryAccount.Balance += @Model.Amount;
                                        TransactionService.AddTransaction(transaction);
                                        transactionStatusMessage = "Transaction completed successfully.";
                                    }
                                } else if (beneficiaryAccount.AccountSupportType == AccountSupportType.MTMX)
                                {
                                    var isFraudulent = await DonwloadPacs008File(senderCustomer, beneficiaryCustomer);
                                    if (!isFraudulent)
                                    {
                                        senderAccount.Balance -= @Model.Amount;
                                        beneficiaryAccount.Balance += @Model.Amount;
                                        TransactionService.AddTransaction(transaction);
                                        transactionStatusMessage = "Transaction completed successfully.";
                                    }
                                }
                                else
                                {
                                    var isFraudulent = await DonwloadPacs008File(senderCustomer, beneficiaryCustomer);
                                    if (!isFraudulent)
                                    {
                                        senderAccount.Balance -= @Model.Amount;
                                        beneficiaryAccount.Balance += @Model.Amount;
                                        TransactionService.AddTransaction(transaction);
                                        transactionStatusMessage = "Transaction completed successfully.";
                                    }
                                }
                                break; 
                        } 
                        break;
                    case TransactionType.SWIFT: 
                        switch (senderAccount.AccountSupportType) 
                        {
                            case AccountSupportType.MTMX:
                                if (beneficiaryAccount.AccountSupportType == AccountSupportType.MT) {
                                    var isFraudulent = await DonwloadMT103File(senderCustomer, beneficiaryCustomer);
                                    if (!isFraudulent)
                                    {
                                        senderAccount.Balance -= @Model.Amount;
                                        beneficiaryAccount.Balance += @Model.Amount;
                                        TransactionService.AddTransaction(transaction);
                                        transactionStatusMessage = "Transaction completed successfully.";
                                    }
                                } else if (beneficiaryAccount.AccountSupportType == AccountSupportType.MX) {
                                    var isFraudulent = await DownloadConvertedToPacs008File(senderCustomer, beneficiaryCustomer);
                                    if (!isFraudulent)
                                    {
                                        senderAccount.Balance -= @Model.Amount;
                                        beneficiaryAccount.Balance += @Model.Amount;
                                        TransactionService.AddTransaction(transaction);
                                        transactionStatusMessage = "Transaction completed successfully.";
                                    }

                                } else {
                                        var isFraudulent = await DonwloadMT103File(senderCustomer, beneficiaryCustomer);
                                        if (!isFraudulent)
                                        {
                                            senderAccount.Balance -= @Model.Amount;
                                            beneficiaryAccount.Balance += @Model.Amount;
                                            TransactionService.AddTransaction(transaction);
                                            transactionStatusMessage = "Transaction completed successfully.";
                                        }
                                }
                                break;
                            case AccountSupportType.MT:
                                if (beneficiaryAccount.AccountSupportType == AccountSupportType.MX) {
                                    var isFraudulent = await DownloadConvertedToPacs008File(senderCustomer, beneficiaryCustomer);
                                    if (!isFraudulent)
                                    {
                                        senderAccount.Balance -= @Model.Amount;
                                        beneficiaryAccount.Balance += @Model.Amount;
                                        TransactionService.AddTransaction(transaction);
                                        transactionStatusMessage = "Transaction completed successfully.";
                                    }
                                } else if (beneficiaryAccount.AccountSupportType == AccountSupportType.MTMX) {
                                        var isFraudulent = await DonwloadMT103File(senderCustomer, beneficiaryCustomer);
                                        if (!isFraudulent)
                                        {
                                            senderAccount.Balance -= @Model.Amount;
                                            beneficiaryAccount.Balance += @Model.Amount;
                                            TransactionService.AddTransaction(transaction);
                                            transactionStatusMessage = "Transaction completed successfully.";
                                        }
                                } else {
                                        var isFraudulent = await DonwloadMT103File(senderCustomer, beneficiaryCustomer);
                                        if (!isFraudulent)
                                        {
                                            senderAccount.Balance -= @Model.Amount;
                                            beneficiaryAccount.Balance += @Model.Amount;
                                            TransactionService.AddTransaction(transaction);
                                            transactionStatusMessage = "Transaction completed successfully.";
                                        }
                                }
                                break;
                            case AccountSupportType.MX:
                                if (beneficiaryAccount.AccountSupportType == AccountSupportType.MT) {
                                    var isFraudulent = await DonwloadConvertedToMT103File(senderCustomer, beneficiaryCustomer);
                                    if (!isFraudulent)
                                    {
                                        senderAccount.Balance -= @Model.Amount;
                                        beneficiaryAccount.Balance += @Model.Amount;
                                        TransactionService.AddTransaction(transaction);
                                        transactionStatusMessage = "Transaction completed successfully.";
                                    }
                                } else if (beneficiaryAccount.AccountSupportType == AccountSupportType.MTMX) {
                                    var isFraudulent = await DonwloadConvertedToMT103File(senderCustomer, beneficiaryCustomer);
                                    if (!isFraudulent)
                                    {
                                        senderAccount.Balance -= @Model.Amount;
                                        beneficiaryAccount.Balance += @Model.Amount;
                                        TransactionService.AddTransaction(transaction);
                                        transactionStatusMessage = "Transaction completed successfully.";
                                    }
                                } else {
                                    transactionStatusMessage = "Payment cannot be processed.";
                                }
                                break; 
                        } 
                        break;
                }
            }
        }
        this.StateHasChanged();
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

    private async Task<bool> DonwloadConvertedToMT103File(Customer sender, Customer beneficiary)
    {
        var pacs008Payment = PaymentService.EnrichPacs008Payment(sender, beneficiary, Model);
        var generatedISO = PaymentService.GeneratePacs008Xml(pacs008Payment);

        var pacs008ConvertedToMT103 = PaymentService.ConvertPacs008ToMT103(generatedISO);
        
        var sanctionCheck = FraudPreventionService.ScanMt103(pacs008ConvertedToMT103);

        if (sanctionCheck)
        {
            transactionStatusMessage = "Payment has been stopped for further investigations.";
            //need to add to the db that customer is fraudulent.

            var fraudCustomer = FraudPreventionService._fraudulentWordFound;


            var victim = new CustomersSanctionStatus()
            {
                CustomerId = sender.CustomerId,
                CustomerStatus = "TO BE REVIEWED",
                FraudulentNamesId = fraudCustomer.Id
            };
            
            
            CustomersSanctionStatusService.AddFraudulentCustomer(victim);
            return sanctionCheck;
        }
        
        var filename = $"ConvertedMT103_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt";
        refreshModel();
        await JSRuntime.InvokeVoidAsync("downloadPayment", filename, "text/plain",
            Convert.ToBase64String(Encoding.UTF8.GetBytes(pacs008ConvertedToMT103)));

        return sanctionCheck;
    }

    private async Task<bool> DownloadConvertedToPacs008File(Customer sender, Customer beneficiary)
    {
        var mt103Payment = PaymentService.EnrichMT103Payment(sender, beneficiary, Model);
        var mt103ConvertedToPacs008 = PaymentService.ConvertMT103ToPacs008(mt103Payment);
        var mt103ConvertedToPacs008File = PaymentService.GeneratePacs008Xml(mt103ConvertedToPacs008);
        
        Pacs008Payment pacs008 = FraudPreventionService.ParsePacs008(mt103ConvertedToPacs008File);
        var sanctionCheck = FraudPreventionService.ScanPacs008(pacs008);

        if (sanctionCheck)
        {
            transactionStatusMessage = "Payment has been stopped for further investigations.";
            //need to add to the db that customer is fraudulent.

            var fraudCustomer = FraudPreventionService._fraudulentWordFound;


            var victim = new CustomersSanctionStatus()
            {
                CustomerId = sender.CustomerId,
                CustomerStatus = "TO BE REVIEWED",
                FraudulentNamesId = fraudCustomer.Id
            };


            CustomersSanctionStatusService.AddFraudulentCustomer(victim);
            return sanctionCheck;
        }

        var filename = $"ConvertedPacs008_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt";
        refreshModel();
        await JSRuntime.InvokeVoidAsync("downloadPayment", filename, "text/plain",
            Convert.ToBase64String(Encoding.UTF8.GetBytes(mt103ConvertedToPacs008File)));
        return sanctionCheck;
    }

    private async Task<bool> DonwloadPacs008File(Customer sender, Customer beneficiary)
    {
        var pacs008Payment = PaymentService.EnrichPacs008Payment(sender, beneficiary, Model);
        var generatedISO = PaymentService.GeneratePacs008Xml(pacs008Payment);

        Pacs008Payment pacs008 = FraudPreventionService.ParsePacs008(generatedISO);
        var sanctionCheck = FraudPreventionService.ScanPacs008(pacs008);

        if (sanctionCheck)
        {
            transactionStatusMessage = "Payment has been stopped for further investigations.";
            //need to add to the db that customer is fraudulent.

            var fraudCustomer = FraudPreventionService._fraudulentWordFound;


            var victim = new CustomersSanctionStatus()
            {
                CustomerId = sender.CustomerId,
                CustomerStatus = "TO BE REVIEWED",
                FraudulentNamesId = fraudCustomer.Id
            };


            CustomersSanctionStatusService.AddFraudulentCustomer(victim);
            return sanctionCheck;
        }

        var filename = $"Pacs008_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt";
        refreshModel();
        await JSRuntime.InvokeVoidAsync("downloadPayment", filename, "text/plain",
            Convert.ToBase64String(Encoding.UTF8.GetBytes(generatedISO)));
        return sanctionCheck;
    }

    private async Task<bool> DonwloadMT103File(Customer sender, Customer beneficiary )
    {
        var mt103Payment = PaymentService.EnrichMT103Payment(sender, beneficiary, Model);
        var generatedSWIFT = PaymentService.GenerateMT103TextFile(mt103Payment);
        
        
        var sanctionCheck = FraudPreventionService.ScanMt103(generatedSWIFT);

        if (sanctionCheck)
        {
            transactionStatusMessage = "Payment has been stopped for further investigations.";
            //need to add to the db that customer is fraudulent.

            var fraudCustomer = FraudPreventionService._fraudulentWordFound;


            var victim = new CustomersSanctionStatus()
            {
                CustomerId = sender.CustomerId,
                CustomerStatus = "TO BE REVIEWED",
                FraudulentNamesId = fraudCustomer.Id
            };
            
            
            CustomersSanctionStatusService.AddFraudulentCustomer(victim);
            return sanctionCheck;
        }
        
        var filename = $"MT103_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt";
        refreshModel();
        await JSRuntime.InvokeVoidAsync("downloadPayment", filename, "text/plain",
            Convert.ToBase64String(Encoding.UTF8.GetBytes(generatedSWIFT)));
        return sanctionCheck;
    }

    public void refreshModel()
    {
        Transaction model = new Transaction();
    }
}