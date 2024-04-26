using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Database;

public interface ITransactionRepository
{
    void AddTransaction(Transaction transaction);
    Transaction GetTransactionById(int transactionId);
    void UpdateTransaction(Transaction transaction);
    void DeleteTransaction(int transactionId);
}