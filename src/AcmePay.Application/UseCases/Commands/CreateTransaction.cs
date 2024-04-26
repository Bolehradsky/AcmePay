using _Common.Utils;
using AcmePay.Domain.Enums;
using AcmePay.Domain.Model;
using AcmePay.Domain.Repositories;
using Mapster;
using MediatR;

namespace AcmePay.Application.UseCases.Commands;

public static class CreateTransaction
{


    public sealed record Contract : IRequest<Result>
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string CardHolderNumber { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        public int CVV { get; set; }
        public string OrderReference { get; set; } = string.Empty;
        public ETransactionStatus TransactionStatus { get; set; }
    }

    public sealed record Result
    {
        public string Id { get; set; } = string.Empty;
        public string OrderReference { get; set; } = string.Empty;
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

            var transaction = Transaction.Create(contract.Amount,
                                                       contract.Currency,
                                                       contract.CardHolderNumber,
                                                       contract.CardHolderName,
                                                       contract.ExpirationMonth,
                                                       contract.ExpirationYear,
                                                       contract.CVV,
                                                       contract.OrderReference,
                                                       contract.TransactionStatus);

            await _transactionRepository.Create(transaction);
            var result = transaction.Adapt<Result>();
            result.Id = EncryptGuid.Encrypt(transaction.Id);
            return result;
        }
    }
}
