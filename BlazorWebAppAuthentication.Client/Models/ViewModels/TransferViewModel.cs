using System.ComponentModel.DataAnnotations;
using BlazorWebAppAuthentication.Client.Components.Pages;
using BlazorWebAppAuthentication.Database;
using BlazorWebAppAuthentication.Domain.Enum;

namespace BlazorWebAppAuthentication.Client.Models.ViewModels;

public class TransferModel
{
    public TransactionType TransactionType { get; set; }
    
    public int SenderAccountId { get; set; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Sender account is required.")]
    [CustomValidation(typeof(TransferModel), nameof(ValidateSenderAccount))]
    public string SenderAccountName { get; set; }
    
    public string BeneficiaryAccountId { get; set; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Beneficiary account is required.")]
    [CustomValidation(typeof(TransferModel), nameof(ValidateBeneficiaryAccountExists))]
    public string BeneficiaryAccountName { get; set; }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter amount.")]
    public decimal Amount { get; set; }
    
    public string RemittenceInfo { get; set; }
    
    public static ValidationResult ValidateBeneficiaryAccountExists(string beneficiaryAccountName, ValidationContext context) 
    {
        if (beneficiaryAccountName == "")
        {
            return new ValidationResult("Beneficiary account is empty.");
        }
        var dbContext = (ApplicationContext)context.GetService(typeof(ApplicationContext));
        var accountExists = dbContext.Accounts.Any(a => a.AccountName == beneficiaryAccountName);
        
        if (!accountExists)
        {
            return new ValidationResult("Beneficiary account does not exist.");
        }
        return ValidationResult.Success;
    }
    public static ValidationResult ValidateSenderAccount(string senderAccountName, ValidationContext context) 
    {
        if (senderAccountName == "")
        {
            return new ValidationResult("Sender account is empty.");
        }
        
        return ValidationResult.Success;
    }
    
}