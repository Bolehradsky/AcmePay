using _Common.Fetch.Pagination;

namespace AcmePay.Api.Controllers.Transactions.Responses;

public class ReadTransactionsPaginateResponse : PaginationResponse<ReadTransactionResponse>
{
    public ReadTransactionsPaginateResponse(int totalCount, int pageSize, int currentPageNumber, List<ReadTransactionResponse> data)
        : base(totalCount, pageSize, currentPageNumber, data)
    {
    }
}





