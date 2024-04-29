namespace BlazorWebAppAuthentication.Client.Payment;

public class MT103Payment
{
    private static readonly Random random = new Random();

    public string SenderReference { get; set; } = GenerateDateString();
    public string BankOperationCode { get; set; } = "CRED";
    public string ValueDateCurrencyAmount { get; set; }
    public string OrderingCustomer { get; set; }
    public string OrderingCustomerName { get; set; }
    public string BeneficiaryCustomer { get; set; }
    public string BeneficiaryCustomerName { get; set; }
    public string RemittanceInformation { get; set; }
    public string Charges { get; set; } = "SHA";
    
    public static string GenerateDateString()
    {
        var now = DateTime.Now;
        
        string datePart = now.ToString("ddMMyyyy");
        string timePart = now.ToString("HHmmss");
        
        int randomNumber = random.Next(1000, 10000); // This ensures the number is always 4 digits
        
        return $"{datePart}{randomNumber}{timePart}";
    }
}

