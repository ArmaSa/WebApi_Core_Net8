using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAppWebApi.Data
{
    public class UnitOfWork<TEntity> : IUnitOfWork<TEntity> where TEntity : class
    {
        protected readonly InvoiceDbContext _DbContext;
        public DbSet<TEntity> Entities { get; }
        public virtual IQueryable<TEntity> Table => Entities;

        public UnitOfWork(InvoiceDbContext dbContext)
        {
            _DbContext = dbContext;
            Entities = _DbContext.Set<TEntity>();
        }

        public void DetachAllEntities()
        {
            var changedEntriesCopy = _DbContext.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                e.State == EntityState.Modified ||
                e.State == EntityState.Deleted || e.State == EntityState.Unchanged)
                .ToList();

            foreach (var entry in changedEntriesCopy)
                entry.State = EntityState.Detached;
        }

        public void Dispose()
        {
            _DbContext.Dispose();
        }

        public IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : class
        {
            return _DbContext.Set<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> AsNoTracking<TEntity>() where TEntity : class
        {
            return _DbContext.Set<TEntity>().AsQueryable().AsNoTracking();
        }

        public int SaveAllChanges()
        {
            return _DbContext.SaveChanges();
        }

        public async Task<int> SaveAllChangesAsync(CancellationToken cancellationToken)
        {
            return await _DbContext.SaveChangesAsync(cancellationToken);
        }

        public int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            return _DbContext.SaveChanges(acceptAllChangesOnSuccess);
        }

        public async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return await _DbContext.SaveChangesAsync(acceptAllChangesOnSuccess,cancellationToken);
        }

        public DbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return _DbContext.Set<TEntity>();
        }

        public async Task AddAsync<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class
        {
            Set<TEntity>().AddRange(entity);
        }

        public async Task AddARangeAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken) where TEntity : class
        {
            Set<TEntity>().AddRange(entities);
        }

        public void Remove<TEntity>(TEntity entity) where TEntity : class
        {
            Set<TEntity>().Remove(entity);
        }

        public void RemoveRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Set<TEntity>().RemoveRange(entities);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            Set<TEntity>().Update(entity);
        }

        public void UpdateRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            Set<TEntity>().UpdateRange(entities);
        }

        public void DeAttach<TEntity>(TEntity entity)
        {
            var entry = _DbContext.Entry(entity);
            if (entity != null)
                entry.State = EntityState.Detached;
        }

        public void Attach<TEntity>(TEntity entity)
            where TEntity : class
        {
            if (_DbContext.Entry(entity).State == EntityState.Detached)
            {
                _DbContext.Set<TEntity>().Attach(entity);
            }
        }
    }
}
