using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebAppAuthentication.Database;

public class TransactionRepository : ITransactionRepository
{
    private readonly ApplicationContext _context;

    public TransactionRepository(ApplicationContext context)
    {
        _context = context;
    }

    public Transaction AddTransaction(Transaction transaction)
    {
         var data = _context.Transactions.Add(transaction);
         _context.SaveChanges();
         return data.Entity;
    }

    public Transaction GetTransactionById(int transactionId)
    {
        var data = _context.Transactions.FirstOrDefault(t => t.TransactionId == transactionId);
        return data;
    }

    public Transaction UpdateTransaction(Transaction transaction)
    {
        var data = _context.Transactions.Update(transaction);
        _context.SaveChanges();
        return data.Entity;
    }

    public Transaction DeleteTransaction(int transactionId)
    {
        var transaction = _context.Transactions.Find(transactionId);
            var data = _context.Transactions.Remove(transaction);
            _context.SaveChanges();
            return data.Entity;
    }
}