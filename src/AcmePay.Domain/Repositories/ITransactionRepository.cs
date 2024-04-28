using AcmePay.Domain.Model;

namespace AcmePay.Domain.Repositories
{
    public interface ITransactionRepository
    {
        Task Authorize(Transaction transaction);
        Task ChangeStatus(Transaction originalTransaction, Transaction changedTransaction);
        Task<Transaction> GetById(Guid id);
    }
}