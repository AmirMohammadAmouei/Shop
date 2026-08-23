using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Transportation.DataAccess.Contexts;
using Transportation.Entities._0.Common;

namespace Transportation.DataAccess._0.Common
{

    public class Repository<T> : IRepository<T>
        where T : class
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext context)
        {
            _context = context;
        }

        public async Task InsertAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(entity, cancellationToken);
        }

        public bool Any(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>().Any(expression);
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().AnyAsync(expression, cancellationToken);
        }

        public IQueryable<T> GetQuery()
        {
            return _context.Set<T>()
                .AsQueryable();
        }

        public async Task<T> GetByIdAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().Where(expression).FirstOrDefaultAsync(cancellationToken);
        }

        public IQueryable<T> GetWhere(Expression<Func<T, bool>> expression)
        {
            return _context.Set<T>()
                .Where(expression)
                .AsNoTracking()
                .AsQueryable();
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Update(entity);
        }
    }

}
