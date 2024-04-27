using AcmePay.Domain.Model;

namespace AcmePay.Domain.Repositories;
public interface ITransactionRepository
{
    Task Authorize(Transaction transaction);
    Task ChangeStatus(Transaction orginalTransaction, Transaction changedTransaction);
    Task<Transaction> GetById(Guid id);
}
