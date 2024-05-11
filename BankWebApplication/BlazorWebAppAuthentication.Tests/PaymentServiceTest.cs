using BlazorWebAppAuthentication.Client.Models.ViewModels;
using BlazorWebAppAuthentication.Client.Payment;
using BlazorWebAppAuthentication.Domain.Entities;
using Microsoft.Extensions.Options;
using Moq;

namespace BlazorWebAppAuthentication.Tests;

public class PaymentServiceTests
{
    private PaymentService _paymentService;
    private Mock<IOptions<MT103Settings>> _mockMt103Settings;

    [SetUp]
    public void Setup()
    {
        _mockMt103Settings = new Mock<IOptions<MT103Settings>>();
        _mockMt103Settings.Setup(s => s.Value).Returns(new MT103Settings
        {
            SenderBankIdentifier = "TESTBANKX",
            ReceiverBankIdentifier = "TESTBANKY",
            DefaultMessageIdentifier = "1234567890",
            BankOperationCode = "CRED",
            Charges = "OUR"
        });

        _paymentService = new PaymentService(_mockMt103Settings.Object);
    }
        [Test]
    public void ConvertPacs008ToMT103_ShouldReturnValidMT103String()
    {
        // Arrange
        string xmlInput = @"<Document xmlns='urn:iso:std:iso:20022:tech:xsd:pacs.008.001.02'>
                              <GrpHdr>
                                <MsgId>MSG0001</MsgId>
                                <CreDtTm>2023-10-04T12:00:00</CreDtTm>
                                <InstgAgt>
                                  <FinInstnId>
                                    <BICFI>TESTBANKX</BICFI>
                                  </FinInstnId>
                                </InstgAgt>
                                <InstdAgt>
                                  <FinInstnId>
                                    <BICFI>TESTBANKY</BICFI>
                                  </FinInstnId>
                                </InstdAgt>
                              </GrpHdr>
                              <CdtTrfTxInf>
                                <Amt>
                                  <InstdAmt Ccy='EUR'>1000.00</InstdAmt>
                                </Amt>
                                <Cdtr>
                                  <Nm>John Doe</Nm>
                                </Cdtr>
                                <CdtrAgt>
                                  <FinInstnId>
                                    <BICFI>TESTBANKY</BICFI>
                                  </FinInstnId>
                                </CdtrAgt>
                                <CdtrAcct>
                                  <Id>
                                    <IBAN>DE89370400440532013000</IBAN>
                                  </Id>
                                </CdtrAcct>
                              </CdtTrfTxInf>
                            </Document>";

        // Act
        var result = _paymentService.ConvertPacs008ToMT103(xmlInput);

        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(result.Contains("TESTBANKX"));
        Assert.IsTrue(result.Contains("1000.00"));
    }
    [Test]
    public void EnrichPacs008Payment_ShouldSetCorrectValues()
    {
      var country = new Country()
      {
        CountryId = 1,
        Name = "Latvia"
      };
      // Arrange
      var sender = new Customer { FirstName = "John", LastName = "Doe", CountryId = 1,City = "New York", Street = "123 Freedom Trail", Zip = "10001" };
      var beneficiary = new Customer();
      var transferModel = new TransferModel { Amount = 1500.00m, BeneficiaryAccountName = "DE89370400440532013000", RemittenceInfo = "Invoice Payment" };

      // Act
      var result = _paymentService.EnrichPacs008Payment(sender, beneficiary, transferModel);

      // Assert
      Assert.AreEqual("EUR", result.Currency);
      Assert.AreEqual(1500.00m, result.Amount);
      Assert.AreEqual("JohnDoe", result.CreditorName);
      Assert.AreEqual(", New York, 123 Freedom Trail, 10001", result.CreditorAddressLine);
    }

}
