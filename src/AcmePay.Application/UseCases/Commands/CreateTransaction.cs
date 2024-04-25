using _Common.Utils;
using AcmePay.Domain.Enums;
using AcmePay.Domain.Model;
using AcmePay.Domain.Repositories;
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
            /// U commands UseCase nemamo DTO (modele) za komunikaciju izmedju Application i Infrastructure,  kao sto je to U Query Usecasevima
            /// Ovde se kreira objekat direktno u Domain i salju mu se parametri pojedinacno, isto vazi i za update

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

            return new Result()
            {
                Id = GuidEncryprion.ScrambleGuid(transaction.Id),
                OrderReference = transaction.OrderReference
            };
        }
    }
}
