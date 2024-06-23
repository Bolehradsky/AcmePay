using _Common.Exceptions;
using AcmePay.Domain.Validation;

namespace AcmePay.Test.Domain.Validation
{
    public class TransactionStatusValidationTests
    {

        [Theory]
        [InlineData("Authorized")]
        [InlineData("Voided")]
        [InlineData("Captured")]

        public void TransactionStatusValidation_Succeed(string transactionStatus)
        {
            // Act
            var exception = Record.Exception(() => TransactionValidation.TransactionStatusValidation(transactionStatus));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void TransactionStatusValidation_ThrwsBussinesRuleValidationException_ForUnknownTransactioStatus()
        {
            // Arrange
            var transactionStatus = "NotExist";

            // Act
            var validateAction = () => TransactionValidation.TransactionStatusValidation(transactionStatus);

            // Assert
            Assert.Throws<BusinessRuleValidationException>(validateAction);
        }

    }
}
