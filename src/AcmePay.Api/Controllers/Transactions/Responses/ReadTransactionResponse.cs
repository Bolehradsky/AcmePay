namespace AcmePay.Api.Controllers.Transactions.Responses
{
    public record ReadTransactionResponse
    {
        public decimal Amount { get; init; }
        public string Currency { get; init; } = string.Empty;
        public string CardHolderNumber { get; init; } = string.Empty;
        public string CardHolderName { get; init; } = string.Empty;
        public string Id { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
    }
}
