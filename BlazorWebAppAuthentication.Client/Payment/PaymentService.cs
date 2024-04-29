using System.Text;
using BlazorWebAppAuthentication.Client.Components.Pages;
using BlazorWebAppAuthentication.Client.Models.ViewModels;
using BlazorWebAppAuthentication.Domain.Entities;
using Microsoft.Extensions.Options;

namespace BlazorWebAppAuthentication.Client.Payment;

public class PaymentService
{
    private readonly MT103Settings _mt103Settings;

    public PaymentService(IOptions<MT103Settings> mt103Settings)
    {
        _mt103Settings = mt103Settings.Value;  
    }
    
    public static readonly Random random = new Random();
    
    public string GenerateMT103TextFile(MT103Payment payment)
    {
        var settings = _mt103Settings;
        var mt103Content = new StringBuilder();
        mt103Content.AppendLine($"{{1:{settings.SenderBankIdentifier}}}{{2:O1031130{DateTime.Now.ToString("yyMMdd")}{settings.ReceiverBankIdentifier}{DateTime.Now.ToString("HHmm")}N}}{{3:{{108:{settings.DefaultMessageIdentifier}}}}}");
        mt103Content.AppendLine("{4:");
        mt103Content.AppendLine($":20:{payment.SenderReference}");
        mt103Content.AppendLine($":23B:{settings.BankOperationCode}");
        mt103Content.AppendLine($":32A:{payment.ValueDateCurrencyAmount}");
        mt103Content.AppendLine($":50A:/{payment.OrderingCustomer}");
        mt103Content.AppendLine(payment.OrderingCustomerName);
        mt103Content.AppendLine($":59:/{payment.BeneficiaryCustomer}");
        mt103Content.AppendLine(payment.BeneficiaryCustomerName);
        mt103Content.AppendLine($":70:{payment.RemittanceInformation}");
        mt103Content.AppendLine($":71A:{settings.Charges}");
        mt103Content.AppendLine("-}");
        return mt103Content.ToString();
    }

    public MT103Payment EnrichMT103Payment(Customer sender, Customer beneficiary, TransferModel model)
    {
        var payment = new MT103Payment()
        {
            SenderReference = GenerateDateString(),
            ValueDateCurrencyAmount = $"{DateTime.Now.ToString("yyMMdd")}EUR{model.Amount}",
            OrderingCustomer = $"{sender.FirstName}",
            OrderingCustomerName = $"{sender.LastName}/{model.SenderAccountName}",
            BeneficiaryCustomer = $"{beneficiary.FirstName}",
            BeneficiaryCustomerName = $"{beneficiary.LastName}/{model.BeneficiaryAccountName}",
            RemittanceInformation = model.RemittenceInfo
        };
        return payment;
    }

    public static string GenerateDateString()
    {
        // Get current date and time
        var now = DateTime.Now;

        // Format date and time
        string datePart = now.ToString("ddMMyyyy");
        string timePart = now.ToString("HHmmss");

        // Generate random 4 digits
        int randomNumber = random.Next(1000, 10000); // This ensures the number is always 4 digits

        // Concatenate parts
        return $"{datePart}{randomNumber}{timePart}";
    }
}