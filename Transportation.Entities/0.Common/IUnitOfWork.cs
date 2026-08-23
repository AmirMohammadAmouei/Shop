namespace Transportation.Entities._0.Common
{
    public interface IUnitOfWork
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
        void Commit();
    }
}
