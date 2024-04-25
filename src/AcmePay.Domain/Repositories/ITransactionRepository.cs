using AcmePay.Domain.Model;

namespace AcmePay.Domain.Repositories;
public interface ITransactionRepository
{
    Task Create(Transaction transaction);
    Task SetStatus(Transaction transaction);
    Task<Transaction> GetById(Guid id);
}
