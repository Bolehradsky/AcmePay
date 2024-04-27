using _Common.Exceptions;
using AcmePay.Domain.Validation;
using AcmePay.Test.Fixtures;

namespace AcmePay.Test.Domain.Validation;

public class ValidateCreditCardTests
{



    [Fact]
    public async Task CreateAuthorizedTransaction_Succeed()
    {
        // Arrange
        var CardHolderName = "Mark Knopfler";
        var CardHolderNumber = CreditCardNumbers.GeRandomCreditcard();
        var ExpirationMonth = 1;
        var ExpirationYear = 2031;
        var CVV = 387;

        // Act
        var exception = Record.Exception(() => CreditCardValidation.Validate(CardHolderNumber, CardHolderName, ExpirationMonth, ExpirationYear, CVV));

        // Assert
        Assert.Null(exception);
    }


    [Fact]
    public async Task CreateAuthorizedTransaction_ThrowsBussinesRulevaidationException_WhenCardHoldernameIsEmpty()
    {
        // Arrange
        var CardHolderName = "";
        var CardHolderNumber = CreditCardNumbers.GeRandomCreditcard();
        var ExpirationMonth = 1;
        var ExpirationYear = 2031;
        var CVV = 387;

        // Act
        var validateAction = () => CreditCardValidation.Validate(CardHolderNumber, CardHolderName, ExpirationMonth, ExpirationYear, CVV);

        // Assert
        Assert.Throws<BusinessRuleValidationException>(validateAction);

    }
}

