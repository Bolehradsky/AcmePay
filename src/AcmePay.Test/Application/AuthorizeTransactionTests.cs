using _Common.Exceptions;
using AcmePay.Application.UseCases.Commands;
using AcmePay.Infrastructure.Repository;
using AcmePay.Test.Fixtures;

namespace AcmePay.Test.Application;

public class AuthorizeTransactionTests
{


    [Fact]
    public async Task CreateAuthorizedTransaction_Succeed()
    {
        // Arrange
        var contract = new AuthorizeTransaction.Contract
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
        var repository = new TransactionRepository(ConnectionProvider.Connect());

        // Act
        var result = await new AuthorizeTransaction.UseCase(repository).Handle(contract, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(contract.OrderReference, result.OrderReference);
    }


    [Fact]
    public async Task CreateAuthorizedTransaction_ThrowsBussinessException_WhenCardHolderNameIsEmpty()
    {
        // Arrange
        var contract = new AuthorizeTransaction.Contract
        {
            Amount = 1023.12M,
            Currency = "USD",
            CardHolderName = String.Empty,
            CardHolderNumber = CreditCardNumbers.GeRandomCreditcard(),
            ExpirationMonth = 12,
            ExpirationYear = 2030,
            CVV = 456,
            OrderReference = $"Test order reference {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")} "
        };
        var repository = new TransactionRepository(ConnectionProvider.Connect());

        // Act
        // Assert
        await Assert.ThrowsAsync<BusinessRuleValidationException>(async () => await new AuthorizeTransaction.UseCase(repository).Handle(contract, CancellationToken.None));
    }
}


