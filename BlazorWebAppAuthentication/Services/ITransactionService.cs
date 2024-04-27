using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Services;

public interface ITransactionService
{
    Task AddTransactionAsync(Transaction transaction);
    Task<Transaction> GetTransactionByIdAsync(int transactionId);
    Task UpdateTransactionAsync(Transaction transaction);
    Task DeleteTransactionAsync(int transactionId);
}