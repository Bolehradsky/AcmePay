namespace AcmePay.Api.Controllers.Transactions.Responses
{
    public record AuthorizeResponse
    {
        public string Id { get; init; } = string.Empty;
        public string OrderReference { get; init; } = string.Empty;
    }
}
