using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using AutoMapper;
using InvoiceAppWebApi.Data;
using InvoiceAppWebApi.FrameworkExtention;
using System.Runtime;
using Microsoft.Extensions.Options;
using InvoiceAppWebApi.Common;

namespace InvoiceAppWebApi.Services.ServiceBase
{
    public abstract class ReadBase<TEntity ,TModel> : IReadBase<TEntity, TModel> where TEntity : class, IKey64 where TModel : class, IKey64
    {
        protected readonly IUnitOfWork<TEntity> _uow;
        protected readonly IMapper _mapper;
        protected readonly ILogger<ReadBase<TEntity , TModel>> _logger;
        protected readonly DbSet<TEntity> _entities;        
        protected readonly IQueryable<TEntity> _query;
        protected readonly IOptions<ApplicationSettings> _appSettings;

        public ReadBase(IMapper mapper, ILogger<ReadBase<TEntity, TModel>> logger, IUnitOfWork<TEntity> uow, IOptions<ApplicationSettings> appSettings)
        {
            _uow = uow;
            _logger = logger;
            _mapper = mapper;
            _appSettings = appSettings;

            _entities = _uow.Set<TEntity>();
            _query = _entities.AsQueryable();
        }

        public TModel GetItemByKey(Int64 id ,params string[] includes)
        {
            var query = this._query.AsNoTracking();

            includes = includes.OrEmptyIfNull().ToArray();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var entityItem = query.Where(e => EF.Property<int>(e, "Id") == id).SingleOrDefault();

            return _mapper.Map<TModel>(entityItem);
        }

        public async Task<TModel> GetItemByKeyAsync(Int64 id, params string[] includes)
        {
            var query = this._query.AsNoTracking();

            includes = includes.OrEmptyIfNull().ToArray();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var entityItem =await query.Where(e => EF.Property<int>(e, "Id") == id).SingleOrDefaultAsync();

            var result =  _mapper.Map<TModel>(entityItem);

            return result;
        }

        public async Task<TEntity> GetEntityItemByKeyAsync(Int64 id, params string[] includes)
        {
            var query = this._query.AsNoTracking();

            includes = includes.OrEmptyIfNull().ToArray();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var entityItem = await query.Where(e => EF.Property<int>(e, "Id") == id).SingleOrDefaultAsync();

            return entityItem;
        }

        public List<TModel> GetItems(Expression<Func<TEntity, bool>> predicate = null, params string[] includes)
        {
            var entityItems = new List<TEntity>();
            var query = this._query.AsNoTracking();

            includes = includes.OrEmptyIfNull().ToArray();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (predicate != null)
                query = query.Where(predicate);

            entityItems = query?.ToList();
            
            return (_mapper.Map<List<TModel>>(entityItems));
        }

        public async Task<List<TModel>> GetItemsAsync(Expression<Func<TEntity, bool>> predicate = null, params string[] includes)
        {
            var entityItems = new List<TEntity>();
            var query = this._query.AsNoTracking();

            includes = includes.OrEmptyIfNull().ToArray();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (predicate != null)
                query = query.Where(predicate);

            entityItems = await query.ToListAsync().ConfigureAwait(false);
            return _mapper.Map<List<TModel>>(entityItems);
        }

        public async Task<List<TEntity>> GetEntityItemsAsync(Expression<Func<TEntity, bool>> predicate = null, params string[] includes)
        {
            var entityItems = new List<TEntity>();
            var query = this._query.AsNoTracking();

            includes = includes.OrEmptyIfNull().ToArray();

            foreach (var include in includes)
            {
                query = query.Include(include);
            }

            if (predicate != null)
                query = query.Where(predicate);

            entityItems = await query.ToListAsync().ConfigureAwait(false);
            return entityItems;
        }

        public async Task<bool> AnyItemsAsync(Expression<Func<TEntity, bool>> predicate = null)
        {
            var myquery = this._query.AsNoTracking();

            if (predicate != null)
                myquery = myquery.Where(predicate);

            return (await myquery.AnyAsync().ConfigureAwait(false));
        }

        public void Dispose()
        {
            //
        }
    }
}