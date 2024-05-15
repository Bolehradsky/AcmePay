using _Common.Utils;
using AcmePay.Domain.Enums;
using AcmePay.Domain.Model;
using AcmePay.Domain.Repositories;
using Mapster;
using MediatR;

namespace AcmePay.Application.UseCases.Commands;

public static class AuthorizeTransaction
{
    public sealed record Contract : IRequest<Result>
    {
        public decimal Amount { get; init; }
        public string Currency { get; init; } = string.Empty;
        public string CardHolderNumber { get; init; } = string.Empty;
        public string CardHolderName { get; init; } = string.Empty;
        public int ExpirationMonth { get; init; }
        public int ExpirationYear { get; init; }
        public int CVV { get; init; }
        public string OrderReference { get; init; } = string.Empty;
        public ETransactionStatus Status { get; init; }
    }

    public sealed record Result
    {
        public string Id { get; set; } = string.Empty;
        public string OrderReference { get; init; } = string.Empty;
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
                                                       contract.Status);

            await _transactionRepository.Authorize(transaction);
            var result = transaction.Adapt<Result>();
            result.Id = EncryptGuid.GetInstance().Encrypt(transaction.Id);
            
            return result;
            }
        }
    }
