using _Common.Utils;
using AcmePay.Domain.Enums;
using AcmePay.Domain.Model;
using AcmePay.Domain.Repositories;
using MediatR;

namespace AcmePay.Application.UseCases.Commands;

public static class SetTransactionStatusVoided
{

    public sealed record Contract : IRequest<Result>
    {
        public string Id { get; set; } = string.Empty;
        public string OrderReference { get; set; } = string.Empty;
    }

    public sealed record Result
    {
        public string Id { get; set; } = string.Empty;
        public string TransactionStatus { get; set; } = string.Empty;
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
            var transaction = await _transactionRepository.GetById(EncryptGuid.Decrypt(contract.Id));
            Transaction.UpdateStatus(transaction, ETransactionStatus.Voided);
            await _transactionRepository.SetStatus(transaction);

            return new Result()
            {
                Id = EncryptGuid.Encrypt(transaction.Id),
                TransactionStatus = transaction.TransactionStatus
            };
        }
    }
}
