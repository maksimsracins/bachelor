using BlazorWebAppAuthentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace BlazorWebAppAuthentication.Database;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationContext _context;

    public TransactionRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task AddTransaction(Transaction transaction)
    {
        await _context.Transactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task<Transaction> GetTransactionById(int transactionId)
    {
        return await _context.Transactions
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.TransactionId == transactionId);
    }

    public async Task UpdateTransaction(Transaction transaction)
    {
        _context.Transactions.Update(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTransaction(int transactionId)
    {
        var transaction = await _context.Transactions.FindAsync(transactionId);
        if (transaction != null)
        {
            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();
        }
    }
}