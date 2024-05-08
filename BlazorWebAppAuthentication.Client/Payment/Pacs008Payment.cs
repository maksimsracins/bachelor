namespace BlazorWebAppAuthentication.Client.Payment;

public class Pacs008Payment
{
    
    private static readonly Random random = new Random();

    public static string GeneratedReference = GenerateDateString();

    public string MessageId { get; set; } = GeneratedReference;
    public DateTime CreationDateTime { get; set; } = DateTime.UtcNow;
    public string NumberOfTransactions { get; set; } = "1";
    public decimal ControlSum { get; set; }
    public string InstructionId { get; set; } = GeneratedReference;
    public string EndToEndId { get; set; } = GeneratedReference;
    public string Currency { get; set; }
    public decimal Amount { get; set; }
    public string RemittenceInfo { get; set; }
    public string DebtorAgentBIC { get; set; }
    public string CreditorAgentBIC { get; set; }
    public string CreditorName { get; set; }
    public string CreditorAddressLine { get; set; }
    public string CreditorAccountIBAN { get; set; }
    
    public static string GenerateDateString()
    {
        var now = DateTime.Now;
        
        string datePart = now.ToString("ddMMyyyy");
        string timePart = now.ToString("HHmmss");
        
        int randomNumber = random.Next(1000, 10000); // This ensures the number is always 4 digits
        
        return $"{datePart}{randomNumber}{timePart}";
    }
    
}