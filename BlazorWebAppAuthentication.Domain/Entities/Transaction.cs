namespace BlazorWebAppAuthentication.Domain.Entities;

public class Transaction
{
    public int TransactionId { get; set; }
    public int SenderId { get; set; }
    public int SenderAccountId { get; set; }
    public int BeneficiaryId { get; set; }
    public int BeneficiaryAccountId { get; set; }
    public int Amount { get; set; }
    public TransactionStatus TransactionStatus { get; set; }
    public string TransactionType { get; set; }
}