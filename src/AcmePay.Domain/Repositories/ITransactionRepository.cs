using AcmePay.Domain.Model;

namespace AcmePay.Domain.Repositories;
public interface ITransactionRepository
{
    Task Create(Transaction transaction);
}
