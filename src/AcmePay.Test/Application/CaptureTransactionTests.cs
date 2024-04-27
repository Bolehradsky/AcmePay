using _Common.Exceptions;
using _Common.Utils;
using AcmePay.Application.UseCases.Commands;
using AcmePay.Domain.Enums;
using AcmePay.Infrastructure.Repository;
using AcmePay.Test.Fixtures;

namespace AcmePay.Test.Application;

public class CaptureTransactionTests
{
    [Fact]
    public async Task CaptureTransaction_Succeed()
    {
        // Arrange
        var orderReference = $"Test order reference {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")} ";
        var contractAuthorize = new AuthorizeTransaction.Contract
        {
            Amount = 1023.12M,
            Currency = "USD",
            CardHolderName = "Bob Marley",
            CardHolderNumber = CreditCardNumbers.GeRandomCreditcard(),
            ExpirationMonth = 12,
            ExpirationYear = 2030,
            CVV = 456,
            OrderReference = orderReference
        };
        var repository = new TransactionRepository(ConnectionProvider.Connect());

        // Act
        var resultAuthorize = await new AuthorizeTransaction.UseCase(repository).Handle(contractAuthorize, CancellationToken.None);

        var contractCapture = new CaptureTransaction.Contract
        {
            Id = resultAuthorize.Id,
            OrderReference = orderReference
        };
        var resultCapture = await new CaptureTransaction.UseCase(repository).Handle(contractCapture, CancellationToken.None);

        // Assert
        Assert.NotNull(resultAuthorize);
        Assert.NotNull(resultCapture);
        Assert.Equal(orderReference, resultAuthorize.OrderReference);
        Assert.Equal(ETransactionStatus.Captured.ToString(), resultCapture.Status);
    }

    [Fact]
    public async Task CaptureTransaction_ThrowsEntityNotFoundException_WhenIdInCaptureRequestIsInvalid()
    {
        // Arrange
        var orderReference = $"Test order reference {DateTime.Now.ToString("yyyy-MM-dd HH-mm-ss")} ";
        var contractAuthorize = new AuthorizeTransaction.Contract
        {
            Amount = 1023.12M,
            Currency = "USD",
            CardHolderName = "Bob Marley",
            CardHolderNumber = CreditCardNumbers.GeRandomCreditcard(),
            ExpirationMonth = 12,
            ExpirationYear = 2030,
            CVV = 456,
            OrderReference = orderReference
        };
        var repository = new TransactionRepository(ConnectionProvider.Connect());

        // Act
        var resultAuthorize = await new AuthorizeTransaction.UseCase(repository).Handle(contractAuthorize, CancellationToken.None);

        var contractCapture = new CaptureTransaction.Contract
        {
            Id = EncryptGuid.Encrypt(Guid.NewGuid()),
            OrderReference = orderReference
        };

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await new CaptureTransaction.UseCase(repository).Handle(contractCapture, CancellationToken.None));

    }

}
