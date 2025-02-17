using InvoiceAppWebApi.Common;
using InvoiceAppWebApi.Domain;
using InvoiceAppWebApi.Services.ServiceBase;
using InvoiceAppWebApi.ViewModel.Invoice;

namespace InvoiceAppWebApi.Services.Interface
{
    public interface IInvoiceService : ICrudBase<Invoice, InvoiceViewModel>
    {
        Task<ServiceResultInfo> CreateAsync(InvoiceInputViewModel modelItem, bool savechange = true);
        Task<ServiceResultModel<List<InvoiceViewModel>>> GetInvoiceByCustomerId(long customerId, params string[] includes);
    }
}
