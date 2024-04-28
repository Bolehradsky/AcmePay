using _Common.Exceptions;
using AcmePay.Domain.Validation;
using AcmePay.Test.Fixtures;

namespace AcmePay.Test.Domain.Validation;

public class CreditCardValidationTests
{



    [Fact]
    public async Task CreateAuthorizedTransaction_Succeed()
    {
        // Arrange
        var cardHolderName = "Mark Knopfler";
        var cardHolderNumber = CreditCardNumbers.GeRandomCreditcard();
        var eExpirationMonth = 1;
        var expirationYear = 2031;
        var cVv = 387;

        // Act
        var exception = Record.Exception(() => CreditCardValidation.Validate(cardHolderNumber, cardHolderName, eExpirationMonth, expirationYear, cVv));

        // Assert
        Assert.Null(exception);
    }


    [Fact]
    public async Task CreateAuthorizedTransaction_ThrowsBussinesRulevaidationException_WhenCardHolderNameIsEmpty()
    {
        // Arrange
        var cardHolderName = "";
        var cardHolderNumber = CreditCardNumbers.GeRandomCreditcard();
        var expirationMonth = 1;
        var expirationYear = 2031;
        var cVv = 387;

        // Act
        var validateAction = () => CreditCardValidation.Validate(cardHolderNumber, cardHolderName, expirationMonth, expirationYear, cVv);

        // Assert
        Assert.Throws<BusinessRuleValidationException>(validateAction);

    }
}

