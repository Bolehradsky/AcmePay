namespace AcmePay.Api.Controllers.Authorize.Responses;

public class TransactionChangeStatusResponse
{
    public string Id { get; set; } = string.Empty;
    public string TransactionStatus { get; set; } = string.Empty;
}
