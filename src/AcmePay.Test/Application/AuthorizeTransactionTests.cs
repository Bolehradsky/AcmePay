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
            OrderReference = $"Test order reference {DateTime.Now.ToString("")} "
        };

        var repository = new TransactionRepository(ConnectionProvider.Connect());

        // Act
        var result = await new AuthorizeTransaction.UseCase(repository).Handle(contract, CancellationToken.None);

        // Assert

        Assert.NotNull(result);
        Assert.Equal(contract.OrderReference, result.OrderReference);
    }
}


