namespace BlazorWebAppAuthentication.Client.Models.ViewModels;

public class TransferModel
{
    public string TransactionType { get; set; }
    public int SenderAccountId { get; set; }
    public string SenderAccountName { get; set; }
    public int BeneficiaryAccountId { get; set; }
    public string BeneficiaryAccountName { get; set; }
    public int Amount { get; set; }
    public string RemittenceInfo { get; set; }
}