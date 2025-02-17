using Microsoft.EntityFrameworkCore;

namespace InvoiceAppWebApi.Data
{
    public interface IUnitOfWork<TEntity> : IDisposable where TEntity : class
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges(bool acceptAllChangesOnSuccess);
        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken());
        int SaveAllChanges();
        void DetachAllEntities();

        Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class;
        Task AddARangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken) where TEntity : class;
        void Remove<TEntity>(TEntity entity) where TEntity : class;
        void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;
        void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void DeAttach<TEntity>(TEntity entity);
        void Attach<TEntity>(TEntity entity) where TEntity : class;

        IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : class;

        IQueryable<TEntity> AsNoTracking<TEntity>() where TEntity : class;

    }
}
