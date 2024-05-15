using _Common.Fetch.Pagination;

namespace AcmePay.Application.Persistence.Queries;

public static class ReadTransactionsModel
    {

    public sealed class RequestModel
        {
        public PaginationRequest? PaginationRequest { get; set; } = default;

        }
    public sealed class ResponseModel : PaginationResponse<ReadTransactionModel>
        {
        public ResponseModel(int totalCount, int pageSize, int currentPageNumber, List<ReadTransactionModel> data)
            : base(totalCount, pageSize, currentPageNumber, data)
            {
            }
        }

    public sealed record ReadTransactionModel
        {
        public decimal Amount { get; init; }
        public string Currency { get; init; } = string.Empty;
        public string CardHolderNumber { get; init; } = string.Empty;
        public string CardHolderName { get; init; } = string.Empty;
        public Guid Id { get; init; }
        public string Status { get; init; } = string.Empty;
        }


    public interface IReadTransactionsQuery
        {
        Task<ReadTransactionsModel.ResponseModel> Execute(ReadTransactionsModel.RequestModel request, CancellationToken cancellationToken = default);
        }
    }

