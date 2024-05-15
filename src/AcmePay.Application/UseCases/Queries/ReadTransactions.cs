using _Common.Fetch.Pagination;
using _Common.Utils;
using Mapster;
using MediatR;
using static AcmePay.Application.Persistence.Queries.ReadTransactionsModel;


namespace AcmePay.Application.UseCases.Queries;

public static class ReadTransactions
{
    public sealed record Contract : IRequest<Result>
    {
        public PaginationRequest PaginationRequest { get; set; } = default!;
    }

    public sealed class Result : PaginationResponse<ReadTransactionResult>
    {
        public Result(int totalCount, int pageSize, int currentPageNumber, List<ReadTransactionResult> data)
            : base(totalCount, pageSize, currentPageNumber, data)
        {
        }
    }

    public sealed record ReadTransactionResult
    {
        public decimal Amount { get; init; }
        public string Currency { get; init; } = string.Empty;
        public string CardHolderNumber { get; init; } = string.Empty;
        public string CardHolderName { get; init; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Status { get; init; } = string.Empty;
    }

    public class UseCase : IRequestHandler<Contract, Result>
    {
        private readonly IReadTransactionsQuery _readTransactionsQuery;

        public UseCase(IReadTransactionsQuery readTransactionsQuery)
        {
            _readTransactionsQuery = readTransactionsQuery;
        }

        public async Task<Result> Handle(Contract contract, CancellationToken cancellation)
        {
            var modelRequest = contract.Adapt<RequestModel>();
            var modelResult = await _readTransactionsQuery.Execute(modelRequest);

            var resultData = new List<ReadTransactionResult>();
            foreach (var item in modelResult.Data)
            {
                var resultItem = item.Adapt<ReadTransactionResult>();
                resultItem.Id = EncryptGuid.GetInstance().Encrypt(item.Id);
                resultData.Add(resultItem);
            }

            var result = new Result(modelResult.TotalCount, modelResult.PageSize, modelResult.CurrentPageNumber, resultData);
            return result;
        }
    }

}



