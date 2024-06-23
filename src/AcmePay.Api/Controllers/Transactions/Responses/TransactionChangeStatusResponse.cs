namespace AcmePay.Api.Controllers.Transactions.Responses;

public record TransactionChangeStatusResponse
{
    public string Id { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
}
