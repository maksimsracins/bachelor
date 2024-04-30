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

public string ConvertPacs008ToMT103(string xmlPacs008)
{
    XDocument xmlDoc;
    try
    {
        xmlDoc = XDocument.Parse(xmlPacs008);
        Console.WriteLine(xmlDoc.ToString());
        var allElements = xmlDoc.Descendants().Select(x => x.Name.LocalName).ToList();
        Console.WriteLine(string.Join(", ", allElements));
    }
    catch (Exception ex)
    {
        throw new ArgumentException("Invalid XML format.", ex);
    }
    
    XNamespace ns = xmlDoc.Root.GetDefaultNamespace();
    var grpHdr = xmlDoc.Descendants(ns + "GrpHdr").FirstOrDefault();
    var cdtTrfTxInf = xmlDoc.Descendants(ns + "CdtTrfTxInf").FirstOrDefault();

    if (grpHdr == null || cdtTrfTxInf == null)
    {
        // Handling missing critical elements by throwing an informative exception
        throw new InvalidOperationException("Required XML elements GrpHdr or CdtTrfTxInf are missing.");
    }

    // Building the MT103 string
    var mt103Content = new StringBuilder();
    mt103Content.AppendLine($"{{1:F01{grpHdr.Element(ns + "InstgAgt").Element(ns + "FinInstnId").Element(ns + "BICFI").Value}XXXXX0000000000}}"); // Sender's BIC
    mt103Content.AppendLine($"{{2:O103{DateTime.Now.ToString("yyMMdd")}{grpHdr.Element(ns + "InstdAgt").Element(ns + "FinInstnId").Element(ns + "BICFI").Value}XXXXX{DateTime.Now.ToString("HHmm")}N}}"); // Receiver's BIC
    mt103Content.AppendLine($"{{3:{{108:{grpHdr.Element(ns + "MsgId").Value}}}}}");
    mt103Content.AppendLine("{4:");
    mt103Content.AppendLine($":20:{grpHdr.Element(ns + "MsgId").Value}");
    mt103Content.AppendLine($":23B:CRED");
    mt103Content.AppendLine($":32A:{DateTime.Parse(grpHdr.Element(ns + "CreDtTm").Value).ToString("yyMMdd")}EUR{cdtTrfTxInf.Element(ns + "Amt").Element(ns + "InstdAmt").Value}");
    mt103Content.AppendLine($":50A:{cdtTrfTxInf.Element(ns + "CdtrAgt").Element(ns + "FinInstnId").Element(ns + "BICFI").Value}");
    mt103Content.AppendLine($"{cdtTrfTxInf.Element(ns + "Cdtr").Element(ns + "Nm").Value}");
    mt103Content.AppendLine($":59:/{cdtTrfTxInf.Element(ns + "CdtrAcct").Element(ns + "Id").Element(ns + "IBAN").Value}{cdtTrfTxInf.Element(ns + "Cdtr").Element(ns + "Nm").Value}");
    mt103Content.AppendLine($":70:Payment for services");
    mt103Content.AppendLine($":71A:SHA");
    mt103Content.AppendLine("-}");

    return mt103Content.ToString();
}


    public Pacs008Payment ConvertMT103ToPacs008(MT103Payment mt103Payment)
    {
      var pacs008 = new Pacs008Payment()
        {
            ControlSum = ParseMT103Amount(mt103Payment.ValueDateCurrencyAmount),
            Currency = "EUR",
            Amount = ParseMT103Amount(mt103Payment.ValueDateCurrencyAmount),
            DebtorAgentBIC = mt103Payment.BankOperationCode,
            CreditorAgentBIC = mt103Payment.BankOperationCode,
            CreditorName = mt103Payment.BeneficiaryCustomer,
            CreditorAddressLine = mt103Payment.BeneficiaryCustomer,
            CreditorAccountIBAN = mt103Payment.BeneficiaryCustomerName,
        };
      return pacs008;
    }

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
            BeneficiaryCustomer = $"{beneficiary.FirstName}{beneficiary.LastName}",
            BeneficiaryCustomerName = $"{model.BeneficiaryAccountName}",
            RemittanceInformation = model.RemittenceInfo
        };
        return payment;
    }
    public decimal ParseMT103Amount(string mt103Amount)
    {
        // The amount follows the 6-digit date and 3-letter currency code in the string
        var amountString = mt103Amount.Substring(9);
        if (decimal.TryParse(amountString, out decimal result))
        {
            return result;
        }
        throw new FormatException("Invalid MT103 amount format.");
    }
    public decimal ParseMT103AmountForEUR(string mt103Amount)
    {
        // Check for EUR in the expected position and proceed with parsing the numerical part
        if (mt103Amount.Substring(6, 3) != "EUR")
            throw new FormatException("Expected currency 'EUR' but found another.");

        var amountString = mt103Amount.Substring(9); // Starts after "yyMMddEUR"
        if (decimal.TryParse(amountString, out decimal result))
        {
            return result;
        }
        throw new FormatException("Invalid amount format.");
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