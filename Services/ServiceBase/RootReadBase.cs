using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using InvoiceAppWebApi.Services.ServiceBase;
using InvoiceAppWebApi.FrameworkExtention;
using InvoiceAppWebApi.Data;


namespace SBS.Services
{
    public abstract class RootReadBase<TEntity> : IRootReadBase<TEntity> where TEntity : class, IKey64
    {
        protected IUnitOfWork<TEntity> _uow;
        protected readonly ILogger<RootReadBase<TEntity>> _logger;
        protected readonly DbSet<TEntity> _entities;
        protected readonly IQueryable<TEntity> _query;

        public RootReadBase(ILogger<RootReadBase<TEntity>> logger,IUnitOfWork<TEntity> uow)
        {
            _uow = uow;
            _logger = logger;
            _entities = _uow.Set<TEntity>();
            _query = _entities.AsQueryable();
        }

        public TEntity GetItemByKey(Int64 id, params string[] includes)
        {
            var query = this._query.AsNoTracking();

            includes = includes.OrEmptyIfNull().ToArray();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var entityItem = query.Where(c => c.Id == id).SingleOrDefault();
            
            return entityItem;
        }

        public async Task<TEntity> GetItemByKeyAsync(Int64 id, params string[] includes)
        {
            var query = this._query.AsNoTracking();

            includes = includes.OrEmptyIfNull().ToArray();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var entityItem = await query.Where(c => c.Id == id).SingleOrDefaultAsync();

            return entityItem;
        }

        public async Task<List<TEntity>> GetItemsAsync(Expression<Func<TEntity, bool>> predicate = null, params string[] includes)
        {
            var query = this._query.AsNoTracking();

            includes = includes.OrEmptyIfNull().ToArray();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (predicate != null)
                query = this._query.Where(predicate);

            var entityItems = await query.ToListAsync().ConfigureAwait(false);
            return entityItems;
        }
        public void Dispose()
        {

        }
    }
}