namespace BlazorWebAppAuthentication.Client.Payment;

public class MT103Settings
{
    public string SenderBankIdentifier { get; set; }
    public string ReceiverBankIdentifier { get; set; }
    public string BankOperationCode { get; set; }
    public string Charges { get; set; }
    public string DefaultMessageIdentifier { get; set; }
}