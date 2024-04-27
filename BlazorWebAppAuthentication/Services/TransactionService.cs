using System.Threading.Tasks;
using BlazorWebAppAuthentication.Domain.Entities;
using BlazorWebAppAuthentication.Database;

namespace BlazorWebAppAuthentication.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public Task AddTransactionAsync(Transaction transaction)
    {
        return _transactionRepository.AddTransaction(transaction);
    }

    public Task<Transaction> GetTransactionByIdAsync(int transactionId)
    {
        return _transactionRepository.GetTransactionById(transactionId);
    }

    public Task UpdateTransactionAsync(Transaction transaction)
    {
        return _transactionRepository.UpdateTransaction(transaction);
    }

    public Task DeleteTransactionAsync(int transactionId)
    {
        return _transactionRepository.DeleteTransaction(transactionId);
    }
}