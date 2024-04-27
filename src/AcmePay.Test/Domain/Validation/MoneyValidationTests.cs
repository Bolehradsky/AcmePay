using AcmePay.Domain.Validation;
using AcmePay.Test.Fixtures;

namespace AcmePay.Test.Domain.Validation
{
    public class MoneyValidationTests
    {

        [Theory]
        [InlineData("USD")]
        [InlineData("EUR")]
        [InlineData("CAD")]
        [InlineData("AUD")]
        [InlineData("RSD")]
        [InlineData("CZK")]
        [InlineData("GBP")]
        [InlineData("JPY")]

        public async Task CurrencyValidation_Succeed(string currency)
        {
            var cardHolderNumber = CreditCardNumbers.GeRandomCreditcard();

            // Act
            var exception = Record.Exception(() => MoneyValidation.Validate(123.12M, currency));

            // Assert
            Assert.Null(exception);
        }

    }
}
