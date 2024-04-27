using _Common.Exceptions;
using AcmePay.Domain.Repositories;
using AcmePay.Infrastructure.Database;
using Dapper;

namespace AcmePay.Infrastructure.Repository;

public class TransactionRepository : ITransactionRepository
{
    private readonly SqlConnectionProvider _connectionProvider;

    public TransactionRepository(SqlConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }


    public async Task Authorize(Domain.Model.Transaction transaction)
    {
        const string sql = @"INSERT INTO [dbo].[Transaction] (Id,Currency,Amount,CardHolderName,CardHolderNumber,ExpirationMonth,ExpirationYear,CVV,OrderReference,Status,CreatedAt,UpdatedAt) "
                           + " values  (@Id,@Currency,@Amount,@CardHolderName,@CardHolderNumber,@ExpirationMonth,@ExpirationYear,@CVV,@OrderReference,@Status,@CreatedAt,@UpdatedAt) ";

        using var connection = _connectionProvider.Create();
        await connection.ExecuteAsync(sql, transaction);
    }


    public async Task<AcmePay.Domain.Model.Transaction> GetById(Guid id)
    {
        const string sql = @"SELECT * FROM  [dbo].[Transaction] WHERE Id=@Id";
        using var connection = _connectionProvider.Create();
        var result = await connection.QueryFirstOrDefaultAsync<AcmePay.Domain.Model.Transaction>(sql, new { Id = id.ToString() });
        if (result is null)
        {
            throw new EntityNotFoundException("Transaction not found!");
        }
        return result;
    }

    public async Task ChangeStatus(Domain.Model.Transaction originalTransaction, Domain.Model.Transaction changedTransaction)
    {
        using var connection = _connectionProvider.Create();
        connection.Open();
        using (var dbTransaction = connection.BeginTransaction())
        {
            try
            {
                const string insertQuery = @"INSERT INTO [dbo].[Transaction] (Id,Currency,Amount,CardHolderName,CardHolderNumber,ExpirationMonth,ExpirationYear,CVV,OrderReference,Status,CreatedAt,UpdatedAt) "
                            + " values  (@Id,@Currency,@Amount,@CardHolderName,@CardHolderNumber,@ExpirationMonth,@ExpirationYear,@CVV,@OrderReference,@Status,@CreatedAt,@UpdatedAt) ";
                await connection.ExecuteAsync(insertQuery, changedTransaction, dbTransaction);

                const string updateQuery = @"UPDATE [dbo].[Transaction] SET Status=@Status,UpdatedAt=@UpdatedAt where Id=@Id";
                await connection.ExecuteAsync(updateQuery, originalTransaction, dbTransaction);

                dbTransaction.Commit();
            }
            catch (Exception ex)
            {

                dbTransaction.Rollback();
                throw new DatabaseException($"Error occured while updatig status : {ex.Message}");
            }

        }


    }
}
