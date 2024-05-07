using _Common.Utils;
using AcmePay.Domain.Enums;
using AcmePay.Domain.Model;
using AcmePay.Domain.Repositories;
using MediatR;

namespace AcmePay.Application.UseCases.Commands;

public static class VoidTransaction
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

            Transaction.UpdateStatus(authorizedTransaction, ETransactionStatus.Voided);

            var voidedTransaction = Transaction.Create(authorizedTransaction.Amount,
                                                      authorizedTransaction.Currency,
                                                      authorizedTransaction.CardHolderNumber,
                                                      authorizedTransaction.CardHolderName,
                                                      authorizedTransaction.ExpirationMonth,
                                                      authorizedTransaction.ExpirationYear,
                                                      authorizedTransaction.CVV,
                                                      authorizedTransaction.OrderReference,
                                                      ETransactionStatus.Voided);

            await _transactionRepository.ChangeStatus(authorizedTransaction, voidedTransaction);
            return new Result()
            {
                Id = EncryptGuid.Encrypt(voidedTransaction.Id),
                Status = voidedTransaction.Status
            };
        }
    }
}
