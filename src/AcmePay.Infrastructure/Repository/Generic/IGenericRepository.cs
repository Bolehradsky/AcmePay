namespace AcmePay.Infrastructure.Repository.Generics
{
    public interface IGenericRepository<T, TId> where T : class where TId : struct, IEquatable<TId>
    {
        bool Add(T entity);
        bool Delete(T entity);
        IEnumerable<T> GetAll();
        Task<T> GetById(TId Id);
        bool Update(T entity);
    }
}