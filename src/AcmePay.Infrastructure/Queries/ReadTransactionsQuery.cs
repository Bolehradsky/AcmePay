using AcmePay.Application.Persistence.Queries;
using AcmePay.Infrastructure.Database;
using Dapper;
using static AcmePay.Application.Persistence.Queries.ReadTransactionsModel;

namespace AcmePay.Infrastructure.Queries
{
    public class ReadTransactionsQuery : IReadTransactionsQuery
    {
        private readonly SqlConnectionProvider _connectionProvider;

        public ReadTransactionsQuery(SqlConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public async Task<ReadTransactionsModel.ResponseModel> Execute(ReadTransactionsModel.RequestModel request, CancellationToken cancellationToken = default)
        {

            int skip = (request!.PaginationRequest.CurrentPage - 1) * request.PaginationRequest.PageSize;
            int take = request.PaginationRequest.PageSize;

            const string sql = @"
                               SELECT Count(*) FROM  [dbo].[Transaction] 
                               SELECT * FROM  [dbo].[Transaction] Order by CreatedAt OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";
            using var connection = _connectionProvider.GetConnection();
            var result = await connection.QueryMultipleAsync(sql, new { Skip = skip, Take = take });

            int totalCount = result.Read<int>().FirstOrDefault();
            List<ReadTransactionsModel.ReadTransactionModel> data = result.Read<ReadTransactionsModel.ReadTransactionModel>().ToList();

            return new ReadTransactionsModel.ResponseModel(totalCount, request.PaginationRequest.PageSize, request.PaginationRequest.CurrentPage, data);


        }
    }
}
