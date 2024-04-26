using BlazorWebAppAuthentication.Domain;
using BlazorWebAppAuthentication.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorWebAppAuthentications.API.Models;

public class ApplicationContext : DbContext 
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        
    }
    
    public DbSet<UserAccount> UserAccounts { get; set; }
    public DbSet<Country> Countries { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
}