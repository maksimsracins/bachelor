using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Domain.Services;

public interface ITransactionService
{
    Transaction AddTransaction(Transaction transaction);
    Transaction GetTransactionById(int transactionId);
    Transaction UpdateTransaction(Transaction transaction);
    Transaction DeleteTransaction(int transactionId);
}