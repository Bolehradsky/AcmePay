using _Common.Exceptions;
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
        {  // Arrange
            var cardHolderNumber = CreditCardNumbers.GeRandomCreditcard();

            // Act
            var exception = Record.Exception(() => MoneyValidation.Validate(123.12M, currency));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task CurrencyValidation_ThrwsBussinesRuleValidationException_ForUnknownCurrencyCode()
        {
            var cardHolderNumber = CreditCardNumbers.GeRandomCreditcard();
            var currency = "ABC";

            var validateAction = () => MoneyValidation.Validate(123.12M, currency);

            // Assert
            Assert.Throws<BusinessRuleValidationException>(validateAction);
        }



    }
}
