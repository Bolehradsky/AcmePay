namespace AcmePay.Api.Controllers.Transactions.Responses
{
    public class ReadTransactionResponse
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string CardHolderNumber { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
        public string Id { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
