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
    private SenderInformation senderInformation { get; set; }

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
        customersSanctionStatusList = GetAllCustomersSanctionStatusList();
        
        //collect sender info from payment
        // senderInformation = new SenderInformation()
        // {
        //     SenderReference = mt103Payment.SenderReference,
        //     OrderingCustomer = mt103Payment.OrderingCustomer,
        //     OrderingCustomerAccount = mt103Payment.BeneficiaryCustomerName,
        //     RemittenceInformation = mt103Payment.RemittanceInformation
        // };

        var result = fraudulentNamesList.Exists(f => f.Name.Contains(
            senderInformation.OrderingCustomer
        ));
        return result;
    }

    public void ScanPacs008(Pacs008Payment pacs008Payment)
    {
        fraudulentNamesList = GetFradulentNames();
        customersSanctionStatusList = GetAllCustomersSanctionStatusList();
    }

    public class SenderInformation
    {
        public string SenderReference { get; set; }
        public string OrderingCustomer { get; set; }
        public string OrderingCustomerAccount { get; set; }
        public string RemittenceInformation { get; set; }
    }
}