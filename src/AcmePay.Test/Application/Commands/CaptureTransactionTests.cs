using _Common.Exceptions;
using _Common.Utils;
using AcmePay.Application.UseCases.Commands;
using AcmePay.Domain.Enums;
using AcmePay.Infrastructure.Repository;
using AcmePay.Test.Fixtures;

namespace AcmePay.Test.Application.Commands;

public class CaptureTransactionTests
{
    [Fact]
    public async Task CaptureTransaction_Succeed()
    {
        // Arrange
        var contractAuthorize = TransactionAuthorizeRequest.Get;
        var repository = new TransactionRepository(ConnectionProvider.Connect());

        // Act
        var resultAuthorize = await new AuthorizeTransaction.UseCase(repository).Handle(contractAuthorize, CancellationToken.None);

        var contractCapture = new CaptureTransaction.Contract
        {
            Id = resultAuthorize.Id,
            OrderReference = contractAuthorize.OrderReference
        };
        var resultCapture = await new CaptureTransaction.UseCase(repository).Handle(contractCapture, CancellationToken.None);

        // Assert
        Assert.NotNull(resultAuthorize);
        Assert.NotNull(resultCapture);
        Assert.Equal(contractAuthorize.OrderReference, resultAuthorize.OrderReference);
        Assert.Equal(ETransactionStatus.Captured.ToString(), resultCapture.Status);
    }

    [Fact]
    public async Task CaptureTransaction_ThrowsEntityNotFoundException_WhenIdInCaptureRequestIsInvalid()
    {
        // Arrange
        var contractAuthorize = TransactionAuthorizeRequest.Get;
        var repository = new TransactionRepository(ConnectionProvider.Connect());

        // Act
        var resultAuthorize = await new AuthorizeTransaction.UseCase(repository).Handle(contractAuthorize, CancellationToken.None);

        var contractCapture = new CaptureTransaction.Contract
        {
            Id = EncryptGuid.Encrypt(Guid.NewGuid()),
            OrderReference = contractAuthorize.OrderReference
        };

        // Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(async () => await new CaptureTransaction.UseCase(repository).Handle(contractCapture, CancellationToken.None));

    }

    [Fact]
    public async Task CaptureTransaction_ThrowsBussinesRuleValidationException_TransactionStatusIcNotAuthorizedAlreadyCapturedWhenIdInCaptureRequestIsInvalid()
    {
        // Arrange
        var contractAuthorize = TransactionAuthorizeRequest.Get;
        var repository = new TransactionRepository(ConnectionProvider.Connect());

        // Act
        var resultAuthorize = await new AuthorizeTransaction.UseCase(repository).Handle(contractAuthorize, CancellationToken.None);

        var contractCapture = new CaptureTransaction.Contract
        {
            Id = resultAuthorize.Id,
            OrderReference = contractAuthorize.OrderReference
        };

        var firsCaptureResult = await new CaptureTransaction.UseCase(repository).Handle(contractCapture, CancellationToken.None);
        // Assert
        await Assert.ThrowsAsync<BusinessRuleValidationException>(async () => await new CaptureTransaction.UseCase(repository).Handle(contractCapture, CancellationToken.None));

    }

}
