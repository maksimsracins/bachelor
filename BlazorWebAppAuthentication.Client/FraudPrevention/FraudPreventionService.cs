using System.Text.RegularExpressions;
using System.Xml.Linq;
using BlazorWebAppAuthentication.Client.Payment;
using BlazorWebAppAuthentication.Client.Services;
using BlazorWebAppAuthentication.Client.Services.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Client.FraudPrevention;

public class FraudPreventionService
{
    private readonly ICustomersSanctionStatusService _customersSanctionStatusService;
    private readonly IFraudulentNamesService _fraudulentNamesService;

    private List<FraudulentNames> fraudulentNamesList = new ();
    private List<CustomersSanctionStatus> customersSanctionStatusList = new ();
    public FraudulentWordFound _fraudulentWordFound = new();

    public FraudPreventionService(ICustomersSanctionStatusService customersSanctionStatusService, IFraudulentNamesService fraudulentNamesService)
    {
        _customersSanctionStatusService = customersSanctionStatusService;
        _fraudulentNamesService = fraudulentNamesService;
    }

    public List<CustomersSanctionStatus> GetAllCustomersSanctionStatusList()
    {
        return _customersSanctionStatusService.GetAllCustomersSanctionStatusList();
    }

    public List<FraudulentNames> GetFradulentNames()
    {
        return _fraudulentNamesService.GetAllFraudulentNames();
    }
    
    public bool ScanMt103(string mt103Payment)
    {
        fraudulentNamesList = GetFradulentNames();
        //if fraudulent name is found -> add new entry in CustomersSanctionList and reject the payment.
        //If sender is registered in CustomersSanctionList db, has status ALLOWED and certain word is mentioned,
        //then payment can go through.
        
        var payment = new MT103Payment();
        
        var regexPattern = new Regex(
            @":20:(?<SenderReference>[\S\s]+?)\n" + // Non-greedy match up to the first newline
            @":23B:(?<BankOperationCode>\w+)\n" +
            @":32A:(?<ValueDateCurrencyAmount>\d{6}[A-Z]{3}\d+,\d*|[\S\s]+?)\n" + // Allow for optional decimal
            @":50A:(?<OrderingCustomer>[\S\s]+?)\n" + // Non-greedy match, captures until the next field
            @"(?<OrderingCustomerName>[\S\s]+?)\n" + // Captures multiple lines until the next field
            @":59:(?<BeneficiaryCustomer>\/[\S\s]+?)\n" + // Non-greedy, captures multiple lines
            @"(?<BeneficiaryCustomerName>[\S\s]+?)\n" + // Captures the name up to the next field
            @":70:(?<RemittanceInformation>[\S\s]+?)\n" + // Specifically designed to capture field 70 correctly
            @":71A:(?<Charges>\w+)",
            RegexOptions.Singleline);
        
        var match = regexPattern.Match(mt103Payment);
        
        if (match.Success)
        {
            payment.SenderReference = match.Groups["SenderReference"].Value.Trim();
            payment.BankOperationCode = match.Groups["BankOperationCode"].Value.Trim();
            payment.ValueDateCurrencyAmount = match.Groups["ValueDateCurrencyAmount"].Value.Trim();
            payment.OrderingCustomer = match.Groups["OrderingCustomer"].Value.Trim();
            payment.BeneficiaryCustomerName = match.Groups["BeneficiaryCustomerName"].Value.Trim();
            payment.RemittanceInformation = match.Groups["RemittanceInformation"].Value.Trim();
            payment.Charges = match.Groups["Charges"].Value.Trim();
        }
        else
        {
            // Handle case where regex does not match
            throw new InvalidOperationException("MT103 format not recognized.");
        }
        

        var result = fraudulentNamesList.Exists(f => f.Name.Contains(
            payment.RemittanceInformation
        ));

        if (result)
        {
            var wordId = fraudulentNamesList.FirstOrDefault(f => f.Name.Contains(
                payment.RemittanceInformation)).Id;
            var word = fraudulentNamesList.FirstOrDefault(f => f.Id == wordId).Name;

            _fraudulentWordFound.Id = wordId;
            _fraudulentWordFound.Word = word;
        }
        return result;
    }
    public static Pacs008Payment ParsePacs008(string xmlText)
    {
        XDocument xmlDoc = XDocument.Parse(xmlText);
        XNamespace ns = "urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02";

        var pacs008Payment = new Pacs008Payment
        {
            MessageId = xmlDoc.Descendants("MsgId").FirstOrDefault()?.Value,
            CreationDateTime = Convert.ToDateTime(xmlDoc.Descendants("CreDtTm").FirstOrDefault()?.Value),
            NumberOfTransactions = xmlDoc.Descendants("NbOfTxs").FirstOrDefault()?.Value ?? "0",
            ControlSum = decimal.Parse(xmlDoc.Descendants("CtrlSum").FirstOrDefault()?.Value ?? "0"),
            InstructionId = xmlDoc.Descendants("InstrId").FirstOrDefault()?.Value,
            EndToEndId = xmlDoc.Descendants("EndToEndId").FirstOrDefault()?.Value,
            Amount = decimal.Parse(xmlDoc.Descendants("InstdAmt").FirstOrDefault()?.Attribute("Ccy")?.Parent?.Value ?? "0"),
            Currency = xmlDoc.Descendants("InstdAmt").FirstOrDefault()?.Attribute("Ccy")?.Value,
            CreditorName = xmlDoc.Descendants("Nm").FirstOrDefault()?.Value,
            CreditorAccountIBAN = xmlDoc.Descendants("IBAN").FirstOrDefault()?.Value,
            RemittenceInfo = xmlDoc.Descendants("RmtInf").Elements("Ustrd").FirstOrDefault()?.Value
        };

        return pacs008Payment;
    }

    public bool ScanPacs008(Pacs008Payment pacs008Payment)
    {
        fraudulentNamesList = GetFradulentNames();

        var result = fraudulentNamesList.Exists(f => f.Name.Contains(
            pacs008Payment.RemittenceInfo
        ));

        if (result)
        {
            var wordId = fraudulentNamesList.FirstOrDefault(f => f.Name.Contains(
                pacs008Payment.RemittenceInfo)).Id;
            var word = fraudulentNamesList.FirstOrDefault(f => f.Id == wordId).Name;

            _fraudulentWordFound.Id = wordId;
            _fraudulentWordFound.Word = word;
        }
        return result;
    }
}
public class FraudulentWordFound
{
    public int Id { get; set; }
    public string Word { get; set; }
}