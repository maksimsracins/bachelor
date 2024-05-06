using BlazorWebAppAuthentication.Client.Services.Interfaces;
using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Client.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;

    public TransactionService(ITransactionRepository transactionRepository)
    {
        _transactionRepository = transactionRepository;
    }

    public Transaction AddTransaction(Transaction transaction)
    {
        return _transactionRepository.AddTransaction(transaction);
    }

    public Transaction GetTransactionById(int transactionId)
    {
        return _transactionRepository.GetTransactionById(transactionId);
    }

    public Transaction UpdateTransaction(Transaction transaction)
    {
        return _transactionRepository.UpdateTransaction(transaction);
    }

    public Transaction DeleteTransaction(int transactionId)
    {
        return _transactionRepository.DeleteTransaction(transactionId);
    }

    public List<Transaction> GetAllTransactions()
    {
        return _transactionRepository.GetAllTransactions();
    }
}