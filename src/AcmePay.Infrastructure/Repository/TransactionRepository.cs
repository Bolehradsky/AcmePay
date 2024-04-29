using _Common.Exceptions;
using AcmePay.Domain.Model;
using AcmePay.Domain.Repositories;
using AcmePay.Infrastructure.Database;
using AcmePay.Infrastructure.Repository.Generics;
using Dapper;

namespace AcmePay.Infrastructure.Repository;

public class TransactionRepository : DapperGenericRepository<Transaction, Guid>, ITransactionRepository
{

    private readonly SqlConnectionProvider _connectionProvider;

    public TransactionRepository(SqlConnectionProvider connectionProvider) : base(connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task Authorize(Transaction transaction)
    {
        await base.Add(transaction);
    }

    public async Task ChangeStatus(Domain.Model.Transaction originalTransaction, Domain.Model.Transaction changedTransaction)
    {
        using var connection = _connectionProvider.GetConnection();
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

    async Task<Transaction> ITransactionRepository.GetById(Guid id)
    {
        return await base.GetById(id);
    }
}
