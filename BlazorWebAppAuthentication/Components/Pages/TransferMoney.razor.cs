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
    
    private string? transactionStatusMessage;
    private List<Domain.Entities.Account> senderAccounts = new ();

    protected override async Task OnInitializedAsync()
    { 
        senderAccounts = await ApplicationContext.Accounts.Where(a => a.AccountId == 1).ToListAsync();
    }
    
    private async Task HandleTransfer()
    {
        var senderAccount = ApplicationContext.Accounts
            .FirstOrDefault(a => a.AccountId == @Model.SenderAccountId);
        var beneficiaryAccount = ApplicationContext.Accounts
            .FirstOrDefault(a => a.AccountId == @Model.BeneficiaryAccountId);

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
        await ApplicationContext.SaveChangesAsync();

        transactionStatusMessage = "Transaction completed successfully.";
        //navigationManager.NavigateTo("/transactionhistory");
    }

    public class TransferModel
    {
        public int SenderAccountId { get; set; }
        public int BeneficiaryAccountId { get; set; }
        public int Amount { get; set; }
    }
}