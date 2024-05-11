using BlazorWebAppAuthentication.Client.FraudPrevention;
using BlazorWebAppAuthentication.Client.Payment;
using BlazorWebAppAuthentication.Client.Services.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;
using Moq;
using Assert = NUnit.Framework.Assert;

namespace BlazorWebAppAuthentication.Tests;
[TestFixture]
public class FraudPreventionServiceTests
{
    private Mock<ICustomersSanctionStatusService> _mockSanctionStatusService;
    private Mock<IFraudulentNamesService> _mockFraudulentNamesService;
    private FraudPreventionService _fraudPreventionService;

    [SetUp]
    public void Setup()
    {
        _mockSanctionStatusService = new Mock<ICustomersSanctionStatusService>();
        _mockFraudulentNamesService = new Mock<IFraudulentNamesService>();
        _fraudPreventionService =
            new FraudPreventionService(_mockSanctionStatusService.Object, _mockFraudulentNamesService.Object);

        // Setup dummy data
        _mockFraudulentNamesService.Setup(service => service.GetAllFraudulentNames()).Returns(new List<FraudulentNames>
        {
            new FraudulentNames { Id = 1, Name = "John Doe" },
            new FraudulentNames { Id = 2, Name = "Jane Doe" }
        });

        _mockSanctionStatusService.Setup(service => service.GetAllCustomersSanctionStatusList()).Returns(
            new List<CustomersSanctionStatus>
            {
                new CustomersSanctionStatus { Id = 1, CustomerId = 1, CustomerStatus = "ALLOWED" },
                new CustomersSanctionStatus { Id = 2, CustomerId = 2, CustomerStatus = "BLOCKED" }
            });
    }
    [Test]
    public void ScanMt103_FraudulentNameDetected_ReturnsTrue()
    {
        // Arrange
        var mt103Payment = ":20:12345\r\n:23B:CRED\r\n:32A:210505USD123456\r\n:50A:John Doe\r\nJane Doe\r\n:59:/123456789\r\n:70:John Doe\r\n:71A:OUR\r\n";

        // Act
        bool result = _fraudPreventionService.ScanMt103(mt103Payment);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual("John Doe", _fraudPreventionService._fraudulentWordFound.Word);
    }

    [Test]
    public void ScanMt103_NoFraudulentNameDetected_ReturnsFalse()
    {
        // Arrange
        var mt103Payment = ":20:12345\r\n:23B:CRED\r\n:32A:210505USD123456\r\n:50A:No Fraud\r\nNon-Fraudulent Name\r\n:59:/123456789\r\n:70:Payment for services\r\n:71A:OUR\r\n";

        // Act
        bool result = _fraudPreventionService.ScanMt103(mt103Payment);

        // Assert
        Assert.False(result);
    }
        [Test]
    public void ScanPacs008_FraudulentNameDetected_ReturnsTrue()
    {
        // Arrange
        var xmlText = @"
            <Document xmlns:urn=""iso:std:iso:20022:tech:xsd:pacs.008.001.02"">
  <FIToFICstmrCdtTrf>
    <GrpHdr>
      <MsgId>080520247591233304</MsgId>
      <CreDtTm>2024-05-08T20:33:08Z</CreDtTm>
      <NbOfTxs>1</NbOfTxs>
      <CtrlSum>1</CtrlSum>
      <InstgAgt>
        <FinInstnId>
          <BICFI>BANKBEBBA</BICFI>
        </FinInstnId>
      </InstgAgt>
      <InstdAgt>
        <FinInstnId>
          <BICFI>BANKBEBBA</BICFI>
        </FinInstnId>
      </InstdAgt>
    </GrpHdr>
    <CdtTrfTxInf>
      <PmtId>
        <InstrId>080520247591233304</InstrId>
        <EndToEndId>080520247591233304</EndToEndId>
      </PmtId>
      <Amt>
        <InstdAmt Ccy=""EUR"">1</InstdAmt>
      </Amt>
      <CdtrAgt>
        <FinInstnId>
          <BICFI>BANKBEBBA</BICFI>
        </FinInstnId>
      </CdtrAgt>
      <Cdtr>
        <Nm>Test2Test2</Nm>
        <PstlAdr>
          <AdrLine>, test2, test2, test2</AdrLine>
        </PstlAdr>
      </Cdtr>
      <CdtrAcct>
        <Id>
          <IBAN>5555852306</IBAN>
        </Id>
      </CdtrAcct>
      <RmtInf>
        <Ustrd>John Doe</Ustrd>
      </RmtInf>
    </CdtTrfTxInf>
  </FIToFICstmrCdtTrf>
</Document>";

        Pacs008Payment pacs008Payment = FraudPreventionService.ParsePacs008(xmlText);

        // Act
        bool result = _fraudPreventionService.ScanPacs008(pacs008Payment);

        // Assert
        Assert.IsTrue(result);
        Assert.AreEqual(1, _fraudPreventionService._fraudulentWordFound.Id);
        Assert.AreEqual("John Doe", _fraudPreventionService._fraudulentWordFound.Word);
    }

    [Test]
    public void ScanPacs008_NoFraudulentNameDetected_ReturnsFalse()
    {
        // Arrange
        var xmlText = @"
            <Document xmlns:urn=""iso:std:iso:20022:tech:xsd:pacs.008.001.02"">
  <FIToFICstmrCdtTrf>
    <GrpHdr>
      <MsgId>080520247591233304</MsgId>
      <CreDtTm>2024-05-08T20:33:08Z</CreDtTm>
      <NbOfTxs>1</NbOfTxs>
      <CtrlSum>1</CtrlSum>
      <InstgAgt>
        <FinInstnId>
          <BICFI>BANKBEBBA</BICFI>
        </FinInstnId>
      </InstgAgt>
      <InstdAgt>
        <FinInstnId>
          <BICFI>BANKBEBBA</BICFI>
        </FinInstnId>
      </InstdAgt>
    </GrpHdr>
    <CdtTrfTxInf>
      <PmtId>
        <InstrId>080520247591233304</InstrId>
        <EndToEndId>080520247591233304</EndToEndId>
      </PmtId>
      <Amt>
        <InstdAmt Ccy=""EUR"">1</InstdAmt>
      </Amt>
      <CdtrAgt>
        <FinInstnId>
          <BICFI>BANKBEBBA</BICFI>
        </FinInstnId>
      </CdtrAgt>
      <Cdtr>
        <Nm>Test2Test2</Nm>
        <PstlAdr>
          <AdrLine>, test2, test2, test2</AdrLine>
        </PstlAdr>
      </Cdtr>
      <CdtrAcct>
        <Id>
          <IBAN>5555852306</IBAN>
        </Id>
      </CdtrAcct>
      <RmtInf>
        <Ustrd>Iran</Ustrd>
      </RmtInf>
    </CdtTrfTxInf>
  </FIToFICstmrCdtTrf>
</Document>";

        Pacs008Payment pacs008Payment = FraudPreventionService.ParsePacs008(xmlText);

        // Act
        bool result = _fraudPreventionService.ScanPacs008(pacs008Payment);

        // Assert
        Assert.False(result);
    }
}

