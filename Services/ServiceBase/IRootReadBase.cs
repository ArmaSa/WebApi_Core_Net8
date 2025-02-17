using InvoiceAppWebApi.FrameworkExtention;
using System.Linq.Expressions;

namespace InvoiceAppWebApi.Services.ServiceBase
{
    public interface IRootReadBase<TEntity> : System.IDisposable where TEntity : class, IKey64
    {
        TEntity GetItemByKey(Int64 id, params string[] includes);

        Task<TEntity> GetItemByKeyAsync(Int64 id, params string[] includes);        

        Task<List<TEntity>> GetItemsAsync(Expression<Func<TEntity, bool>> predicate = null, params string[] includes);
    }
}