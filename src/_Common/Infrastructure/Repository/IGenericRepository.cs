namespace _Common.Infrastructure.Repository
{
    public interface IGenericRepository<T, TId> where T : class where TId : struct, IEquatable<TId>
    {
        Task<bool> Add(T entity);
        Task<bool> Delete(T entity);
        Task<T> GetById(TId Id);
        Task<bool> Update(T entity);
        IEnumerable<T> GetAll();
        }
}