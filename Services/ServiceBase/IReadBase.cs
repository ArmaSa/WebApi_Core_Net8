using System.Linq.Expressions;

namespace InvoiceAppWebApi.Services.ServiceBase
{
    public interface IReadBase<TEntity, TModel> : System.IDisposable where TEntity : class
    {
        TModel GetItemByKey(Int64 id, params string[] includes);

        Task<TModel> GetItemByKeyAsync(Int64 id, params string[] includes);

        Task<TEntity> GetEntityItemByKeyAsync(Int64 id, params string[] includes);

        Task<List<TModel>> GetItemsAsync(Expression<Func<TEntity, bool>> predicate = null, params string[] includes);

        Task<List<TEntity>> GetEntityItemsAsync(Expression<Func<TEntity, bool>> predicate = null, params string[] includes);

        Task<bool> AnyItemsAsync(Expression<Func<TEntity, bool>> predicate = null);
    }
}