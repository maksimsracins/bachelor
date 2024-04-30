using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Database.Interfaces;

public interface ITransactionRepository
{
    Transaction AddTransaction(Transaction transaction);
    Transaction GetTransactionById(int transactionId);
    Transaction UpdateTransaction(Transaction transaction);
    Transaction DeleteTransaction(int transactionId);
    List<Transaction> GetAllTransactions();
}