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
        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string CardHolderNumber { get; set; } = string.Empty;
        public string CardHolderName { get; set; } = string.Empty;
        public Guid Id { get; set; }
        public string Status { get; set; } = string.Empty;
    }


    public interface IReadTransactionsQuery
    {
        Task<ReadTransactionsModel.ResponseModel> Execute(ReadTransactionsModel.RequestModel request, CancellationToken cancellationToken = default);
    }
}

