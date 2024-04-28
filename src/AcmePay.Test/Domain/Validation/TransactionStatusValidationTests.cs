using _Common.Exceptions;

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
            var exception = Record.Exception(() => AcmePay.Domain.Model.Transaction.TransactionStatusValidation(transactionStatus));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public void TransactionStatusValidation_ThrwsBussinesRuleValidationException_ForUnknownTransactioStatus()
        {
            // Arrange
            var transactionStatus = "NotExist";

            // Act
            var validateAction = () => AcmePay.Domain.Model.Transaction.TransactionStatusValidation(transactionStatus);

            // Assert
            Assert.Throws<BusinessRuleValidationException>(validateAction);
        }

    }
}
