using BlazorWebAppAuthentication.Domain.Enum;

namespace BlazorWebAppAuthentication.Domain.Entities;

public class Transaction
{
    public int TransactionId { get; set; }
    public int SenderId { get; set; }
    public int SenderAccountId { get; set; }
    public int BeneficiaryId { get; set; }
    public string BeneficiaryAccountId { get; set; }
    public decimal Amount { get; set; }
    public TransactionStatus TransactionStatus { get; set; }
    public TransactionType TransactionType { get; set; }
    public string RemittanceInfo { get; set; }
}