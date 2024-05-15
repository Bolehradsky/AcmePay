using _Common.Exceptions;
using AcmePay.Domain.Model;
using AcmePay.Domain.Repositories;
using AcmePay.Infrastructure.Database;
using AcmePay.Infrastructure.Repository.Generics;
using Dapper;
using System.Linq;

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
        
        string tableName = base.GetTableName();
        string columns = base.GetColumns(excludeKey: false);
        string properties = base.GetPropertyNames(excludeKey: false);
        string keyColumn = GetKeyColumnName()??"Id";
        string keyProperty = base.GetKeyPropertyName()??"Id";

        using var connection = _connectionProvider.GetConnection();
        connection.Open();
        using (var dbTransaction = connection.BeginTransaction())
        {
            try
            {

                string query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties})";
                await connection.ExecuteAsync(query, changedTransaction, dbTransaction);
                                
                query = $"UPDATE {tableName}  SET Status=@Status,UpdatedAt=@UpdatedAt " +
                                             $"WHERE {keyColumn} = @{keyProperty}";
                
                await connection.ExecuteAsync(query, originalTransaction, dbTransaction);

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
