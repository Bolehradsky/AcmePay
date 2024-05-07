using _Common.Utils;
using AcmePay.Domain.Enums;
using AcmePay.Domain.Model;
using AcmePay.Domain.Repositories;
using MediatR;

namespace AcmePay.Application.UseCases.Commands;

public static class CaptureTransaction
{

    public sealed record Contract : IRequest<Result>
    {
        public string Id { get; set; } = string.Empty;
        public string OrderReference { get; set; } = string.Empty;
    }

    public sealed record Result
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
    public class UseCase : IRequestHandler<Contract, Result>
    {
        private readonly ITransactionRepository _transactionRepository;

        public UseCase(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<Result> Handle(Contract contract, CancellationToken cancellationToken)
        {
            var authorizedTransaction = await _transactionRepository.GetById(EncryptGuid.Decrypt(contract.Id));

            Transaction.UpdateStatus(authorizedTransaction, ETransactionStatus.Captured);

            var capturedTransaction = Transaction.Create(authorizedTransaction.Amount,
                                                      authorizedTransaction.Currency,
                                                      authorizedTransaction.CardHolderNumber,
                                                      authorizedTransaction.CardHolderName,
                                                      authorizedTransaction.ExpirationMonth,
                                                      authorizedTransaction.ExpirationYear,
                                                      authorizedTransaction.CVV,
                                                      authorizedTransaction.OrderReference,
                                                      ETransactionStatus.Captured);

            await _transactionRepository.ChangeStatus(authorizedTransaction, capturedTransaction);
            return new Result()
            {
                Id = EncryptGuid.Encrypt(capturedTransaction.Id),
                Status = capturedTransaction.Status
            };
        }
    }
}
