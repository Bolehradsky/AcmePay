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


    public async Task Create(Domain.Model.Transaction transaction)
    {
        const string sql = @"INSERT INTO [dbo].[Transaction] (Id,Currency,Ammount,CardHolderName,CardHolderNumber,ExpirationMonth,ExpirationYear,CVV,OrderReference,TransactionStatus,CreatedAt,UpdatedAt) "
                           + " values  (@Id,@Currency,@Amount,@CardHolderName,@CardHolderNumber,@ExpirationMonth,@ExpirationYear,@CVV,@OrderReference,@TransactionStatus,@CreatedAt,@UpdatedAt) ";

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

    public async Task SetStatus(Domain.Model.Transaction transaction)
    {
        const string sql = @"UPDATE [dbo].[Transaction] SET TransactionStatus=@TransactionStatus where Id=@Id";
        using var connection = _connectionProvider.Create();
        await connection.ExecuteAsync(sql, transaction);
    }


}
