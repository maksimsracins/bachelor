namespace BlazorWebAppAuthentication.Domain;

public enum TransactionStatus
{
    Processed = 1,
    Aborted = 2,
    Cancelled = 3,
    ToBeChecked = 4
}