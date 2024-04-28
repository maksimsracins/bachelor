using BlazorWebAppAuthentication.Domain;
using BlazorWebAppAuthentication.Domain.Entities;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebAppAuthentication.Components.Pages;

public partial class TransferMoney
{
    [CascadingParameter]
    public HttpContext? HttpContext { get; set; }

    [SupplyParameterFromForm] 
    public TransferModel Model { get; set; } = new();

    public decimal accountBalance = 0;
    
    private Domain.Entities.Account selectedSenderAccount;
    private bool showTransferForm = true;
    
    private string? transactionStatusMessage;
    private List<Domain.Entities.Account> senderAccounts = new ();

    protected override async Task OnInitializedAsync()
    { 
        senderAccounts = ApplicationContext.Accounts.Where(a => a.AccountId == 1).ToList();
    }
    
    private async Task HandleTransfer()
    {
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

        var transaction = new Transaction
        {
            SenderAccountId = @Model.SenderAccountId,
            BeneficiaryAccountId = @Model.BeneficiaryAccountId,
            Amount = @Model.Amount,
            TransactionStatus = TransactionStatus.Processed, // Assuming instant completion
            TransactionType = "MT103"
        };

        ApplicationContext.Transactions.Add(transaction);
          ApplicationContext.SaveChangesAsync();

        transactionStatusMessage = "Transaction completed successfully.";
        //navigationManager.NavigateTo("/transactionhistory");
    }

    public class TransferModel
    {
        public string TransactionType { get; set; }
        public int SenderAccountId { get; set; }
        public string SenderAccountName { get; set; }
        public int BeneficiaryAccountId { get; set; }
        public string BeneficiaryAccountName { get; set; }
        public int Amount { get; set; }
    }
}