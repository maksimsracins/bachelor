using System.Text;
using System.Xml.Linq;
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


    public Pacs008Payment EnrichPacs008Payment(Customer sender, Customer beneficiary, TransferModel model)
    {
        var pacs008 = new Pacs008Payment()
        {
            ControlSum = model.Amount,
            Currency = "EUR",
            Amount = model.Amount,
            DebtorAgentBIC = "BANKBEBBA",
            CreditorAgentBIC = "BANKBEBBA",
            CreditorName = $"{sender.FirstName}{sender.LastName}",
            CreditorAddressLine = $"{sender.Country}, {sender.City}, {sender.Street}, {sender.Zip}",
            CreditorAccountIBAN = model.BeneficiaryAccountName
        };
        return pacs008;
    }
    
    
    public string GeneratePacs008Xml(Pacs008Payment payment)
    {
        var xmlDoc = new XDocument(
            new XElement("Document",
                new XAttribute(XNamespace.Xmlns + "urn", "iso:std:iso:20022:tech:xsd:pacs.008.001.02"),
                new XElement("FIToFICstmrCdtTrf",
                    new XElement("GrpHdr",
                        new XElement("MsgId", payment.MessageId),
                        new XElement("CreDtTm", payment.CreationDateTime.ToString("yyyy-MM-ddTHH:mm:ssZ")),
                        new XElement("NbOfTxs", payment.NumberOfTransactions),
                        new XElement("CtrlSum", payment.ControlSum),
                        new XElement("InstgAgt",
                            new XElement("FinInstnId",
                                new XElement("BICFI", payment.DebtorAgentBIC))),
                        new XElement("InstdAgt",
                            new XElement("FinInstnId",
                                new XElement("BICFI", payment.CreditorAgentBIC)))),
                    new XElement("CdtTrfTxInf",
                        new XElement("PmtId",
                            new XElement("InstrId", payment.InstructionId),
                            new XElement("EndToEndId", payment.EndToEndId)),
                        new XElement("Amt",
                            new XElement("InstdAmt", payment.Amount,
                                new XAttribute("Ccy", payment.Currency))),
                        new XElement("CdtrAgt",
                            new XElement("FinInstnId",
                                new XElement("BICFI", payment.CreditorAgentBIC))),
                        new XElement("Cdtr",
                            new XElement("Nm", payment.CreditorName),
                            new XElement("PstlAdr",
                                new XElement("AdrLine", payment.CreditorAddressLine))),
                        new XElement("CdtrAcct",
                            new XElement("Id",
                                new XElement("IBAN", payment.CreditorAccountIBAN)))))));
    
        return xmlDoc.ToString();
    }
    
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