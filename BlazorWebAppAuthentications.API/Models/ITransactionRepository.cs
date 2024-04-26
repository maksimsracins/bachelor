using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentications.API.Models;

public interface ITransactionRepository
{
    IEnumerable<Transaction> GetTransactionsBySenderId(int id);
    IEnumerable<Transaction> GetTransactionsByBeneficiaryId(int id);
    IEnumerable<Transaction> GetTransactionsByAmount(int amount);
    Transaction GetTransactionById(int id);
    IEnumerable<Transaction> GetALLTransactionsByTransactionType { get; set; }
    IEnumerable<Transaction> GetALLTransactionsByAmount(int amount);
    Transaction GetTransaction(Transaction transaction);

}