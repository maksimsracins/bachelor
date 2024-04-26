using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Database;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationContext _context;

    public TransactionRepository(ApplicationContext context)
    {
        _context = context;
    }

    public void AddTransaction(Transaction transaction)
    {
        _context.Transactions.Add(transaction);
        _context.SaveChanges();
    }

    public Transaction GetTransactionById(int transactionId)
    {
        return _context.Transactions.FirstOrDefault(t => t.TransactionId == transactionId);
    }

    public void UpdateTransaction(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
        _context.SaveChanges();
    }

    public void DeleteTransaction(int transactionId)
    {
        var transaction = _context.Transactions.Find(transactionId);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            _context.SaveChanges();
        }
    }
}