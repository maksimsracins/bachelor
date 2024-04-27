using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Database;

public interface ITransactionRepository
{
    Task AddTransaction(Transaction transaction);
    Task<Transaction> GetTransactionById(int transactionId);
    Task UpdateTransaction(Transaction transaction);
    Task DeleteTransaction(int transactionId);
}