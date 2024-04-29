using BlazorWebAppAuthentication.Domain;
using BlazorWebAppAuthentication.Domain.Entities;
using BlazorWebAppAuthentication.Domain.Enum;

namespace BlazorWebAppAuthentication.Database;

public class DbInitializer
{
    public static void Seed(IApplicationBuilder builder)
    {
        ApplicationContext context = builder.ApplicationServices.CreateScope()
            .ServiceProvider.GetRequiredService<ApplicationContext>();

        if (!context.Accounts.Any())
        {
            context.AddRange(
                new Account{AccountId = 1, AccountName = "5555852301", AccountType = AccountType.Transfer, CustomerId = 1, Balance = 1000},
                            new Account{AccountId = 2, AccountName = "5555852302", AccountType = AccountType.Transfer, CustomerId = 1, Balance = 1000},
                            new Account{AccountId = 3, AccountName = "5555852303", AccountType = AccountType.Transfer, CustomerId = 1, Balance = 1000},
                            new Account{AccountId = 4, AccountName = "5555852304", AccountType = AccountType.Transfer, CustomerId = 1, Balance = 1000},
                            new Account{AccountId = 5, AccountName = "5555852305", AccountType = AccountType.Transfer, CustomerId = 1, Balance = 1000},
                            new Account{AccountId = 6, AccountName = "5555852306", AccountType = AccountType.Transfer, CustomerId = 1, Balance = 1000},
                            new Account{AccountId = 7, AccountName = "5555852307", AccountType = AccountType.Transfer, CustomerId = 1, Balance = 1000},
                            new Account{AccountId = 8, AccountName = "5555852308", AccountType = AccountType.Transfer, CustomerId = 1, Balance = 1000},
                            new Account{AccountId = 9, AccountName = "5555852309", AccountType = AccountType.Transfer, CustomerId = 1, Balance = 1000},
                            new Account{AccountId = 10, AccountName = "5555852310", AccountType = AccountType.Transfer, CustomerId = 1, Balance = 1000}
                
                );
        }

        if (!context.Transactions.Any())
        {
            context.AddRange(
                    new Transaction{TransactionId = 1, SenderId = 1, SenderAccountId = 1, BeneficiaryId = 2, BeneficiaryAccountId = "1", Amount = 10, TransactionStatus = TransactionStatus.Processed, TransactionType = "MT103"},
                                new Transaction{TransactionId = 2, SenderId = 1, SenderAccountId = 1, BeneficiaryId = 2, BeneficiaryAccountId = "1", Amount = 11, TransactionStatus = TransactionStatus.Processed, TransactionType = "pacs008"},
                                new Transaction{TransactionId = 3, SenderId = 1, SenderAccountId = 1, BeneficiaryId = 2, BeneficiaryAccountId = "1", Amount = 12, TransactionStatus = TransactionStatus.Processed, TransactionType = "pacs008"},
                                new Transaction{TransactionId = 4, SenderId = 1, SenderAccountId = 1, BeneficiaryId = 2, BeneficiaryAccountId = "1", Amount = 13, TransactionStatus = TransactionStatus.Processed, TransactionType = "MT103"},
                                new Transaction{TransactionId = 5, SenderId = 1, SenderAccountId = 1, BeneficiaryId = 2, BeneficiaryAccountId = "1", Amount = 14, TransactionStatus = TransactionStatus.Processed, TransactionType = "MT103"},
                                new Transaction{TransactionId = 6, SenderId = 1, SenderAccountId = 1, BeneficiaryId = 2, BeneficiaryAccountId = "1", Amount = 15, TransactionStatus = TransactionStatus.Processed, TransactionType = "MT103"},
                                new Transaction{TransactionId = 7, SenderId = 1, SenderAccountId = 1, BeneficiaryId = 2, BeneficiaryAccountId = "1", Amount = 16, TransactionStatus = TransactionStatus.Processed, TransactionType = "MT103"},
                                new Transaction{TransactionId = 8, SenderId = 1, SenderAccountId = 1, BeneficiaryId = 2, BeneficiaryAccountId = "1", Amount = 17, TransactionStatus = TransactionStatus.Processed, TransactionType = "MT103"}
                );
        }

        if (!context.Countries.Any())
        {
            context.AddRange(
                new Country{CountryId = 1, Name = "Riga"},
                new Country{CountryId = 2, Name = "Minsk"}
                );
        }
        
        if (!context.UserAccount.Any())
        {
            /*context.AddRange(
                new UserAccount{UserAccountId = 1, Username = "admin", Email = "test@test.com", Password = "admin123", Role = "Administrator"},
                            new UserAccount{UserAccountId = 2, Username = "user", Email = "test@test.com", Password = "user123", Role = "User"}
                );*/
        };

        if (!context.Customers.Any())
        {
            context.AddRange(
                new Customer
                { 
                    CustomerId = 1, 
                    FirstName = "Test1", 
                    LastName = "Test1", 
                    BirthDate = new DateTime(1996,1,16), 
                    Street = "test", 
                    Zip = "test", 
                    City = "test", 
                    CountryId = 1,
                    PhoneNumber = "+313131313131",
                    MaritalStatus = MaritalStatus.Married, 
                    Gender = Gender.Male, 
                    JoinedDate = new DateTime(2024,04,18),
                    ExitDate = null
                },
                new Customer
                { 
                    CustomerId = 2, 
                    FirstName = "Test2", 
                    LastName = "Test2", 
                    BirthDate = new DateTime(1996,1,16), 
                    Street = "test2", 
                    Zip = "test2", 
                    City = "test2", 
                    CountryId = 2,
                    PhoneNumber = "+313131313131",
                    MaritalStatus = MaritalStatus.Married, 
                    Gender = Gender.Male, 
                    JoinedDate = new DateTime(2024,04,18),
                    ExitDate = null
                }
                );
        }


        context.SaveChanges();
    }
}