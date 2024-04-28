using _Common.Exceptions;
using AcmePay.Application.UseCases.Commands;
using AcmePay.Domain.Repositories;
using AcmePay.Test.Fixtures;
using Moq;

namespace AcmePay.Test.Application;

public class AuthorizeTransactionTests
{
    [Fact]
    public async Task CreateAuthorizedTransaction_Succeed()
    {
        // Arrange
        var contract = TransactionAuthorizeRequest.Get;
        var repository = new Mock<ITransactionRepository>().Object;
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
        var contract = TransactionAuthorizeRequest.Get;
        contract.CardHolderName = string.Empty;
        var repository = new Mock<ITransactionRepository>().Object;

        // Act
        // Assert
        await Assert.ThrowsAsync<BusinessRuleValidationException>
            (async () => await new AuthorizeTransaction.UseCase(repository).Handle(contract, CancellationToken.None));
    }
}


