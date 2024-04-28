namespace AcmePay.Infrastructure.Repository.Generics
{
    public interface IGenericRepository<T, TId> where T : class where TId : struct, IEquatable<TId>
    {
        Task<bool> Add(T entity);
        Task<bool> Delete(T entity);
        IEnumerable<T> GetAll();
        Task<T> GetById(TId Id);
        Task<bool> Update(T entity);
    }
}