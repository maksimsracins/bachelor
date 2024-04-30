using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Client.Services;

public interface ITransactionService
{
    Transaction AddTransaction(Transaction transaction);
    Transaction GetTransactionById(int transactionId);
    Transaction UpdateTransaction(Transaction transaction);
    Transaction DeleteTransaction(int transactionId);
    List<Transaction> GetAllTransactions();
}