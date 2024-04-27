using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Database;

public interface ITransactionRepository
{
    Transaction AddTransaction(Transaction transaction);
    Transaction GetTransactionById(int transactionId);
    Transaction UpdateTransaction(Transaction transaction);
    Transaction DeleteTransaction(int transactionId);
}