using AcmePay.Application.UseCases.Commands;

namespace AcmePay.Test.Fixtures
{
    public static class TransactionAuthorizeRequest
    {
        public static AuthorizeTransaction.Contract Get => new AuthorizeTransaction.Contract
        {
            Amount = 1023.12M,
            Currency = "USD",
            CardHolderName = "Bob Marley",
            CardHolderNumber = CreditCardNumbers.GeRandomCreditcard(),
            ExpirationMonth = 12,
            ExpirationYear = 2030,
            CVV = 456,
            OrderReference = $"Test order reference {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")} "
        };
    }
}
