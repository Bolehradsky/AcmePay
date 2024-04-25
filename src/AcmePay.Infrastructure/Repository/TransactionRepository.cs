using AcmePay.Domain.Model;
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


    public async Task Create(Transaction transaction)
    {

        const string sql = @"INSERT INTO [dbo].[Transaction] (Id,Currency,Ammount,CardHolderName,CardHolderNumber,ExpirationMonth,ExpirationYear,CVV,OrderReference,TransactionStatus,CreatedAt,UpdatedAt) " +
                                           " values  (@Id,@Currency,@Amount,@CardHolderName,@CardHolderNumber,@ExpirationMonth,@ExpirationYear,@CVV,@OrderReference,@TransactionStatus,@CreatedAt,@UpdatedAt) ";

        using var connection = _connectionProvider.Create();

        await connection.ExecuteAsync(sql, transaction);


    }

}
