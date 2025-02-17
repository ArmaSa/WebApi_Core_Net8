using InvoiceAppWebApi.Common;
using InvoiceAppWebApi.Domain;
using InvoiceAppWebApi.Services.ServiceBase;
using InvoiceAppWebApi.ViewModel.Customer;

namespace InvoiceAppWebApi.Services.Interface
{
    public interface ICustomerService : ICrudBase<Customer, CustomerViewModel>
    {
    }
}
