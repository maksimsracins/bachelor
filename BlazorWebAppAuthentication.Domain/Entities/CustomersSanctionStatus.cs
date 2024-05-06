namespace BlazorWebAppAuthentication.Domain.Entities;

public class CustomersSanctionStatus
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string? CustomerStatus { get; set; }
    public int FraudulentNamesId { get; set; }
}