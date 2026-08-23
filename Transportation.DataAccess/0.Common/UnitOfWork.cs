using Transportation.DataAccess.Contexts;
using Transportation.Entities._0.Common;

namespace Transportation.DataAccess._0.Common
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly AppDbContext _context;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
