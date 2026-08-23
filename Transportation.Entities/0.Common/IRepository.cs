using System.Linq.Expressions;

namespace Transportation.Entities._0.Common
{
    public interface IRepository<T>
    {
        Task InsertAsync(T entity, CancellationToken cancellationToken = default);
        void Update(T entity);
        void Remove(T entity);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
        IQueryable<T> GetQuery();
        IQueryable<T> GetWhere(Expression<Func<T, bool>> expression);
        Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
        bool Any(Expression<Func<T, bool>> expression);
    }

}
