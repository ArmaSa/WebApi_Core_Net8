using InvoiceAppWebApi.Common;
using InvoiceAppWebApi.FrameworkExtention;

namespace InvoiceAppWebApi.Services.ServiceBase
{
    public interface ICrudBase<TEntity ,TModel> : IReadBase<TEntity ,TModel> where TEntity : class, IKey64 where TModel : class, IKey64
    {
        Task<ServiceResultInfo> CreateAsync(TModel modelItem, bool savechange = true);

        Task<ServiceResultInfo> CreateAsync(IEnumerable<TModel> modelItems, bool savechange = true);

        Task<ServiceResultInfo> UpdateAsync(TModel modelItem, bool savechange = true);

        Task<ServiceResultInfo> UpdateAsync(IEnumerable<TModel> modelItems, bool savechange = true);


        Task<ServiceResultInfo> DeleteByKeysAsync(Int64 id ,bool savechange = true);

        Task<ServiceResultInfo> DeleteByKeysAsync(List<Int64> ids, bool savechange = true);
    }
}