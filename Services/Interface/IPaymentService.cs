using InvoiceAppWebApi.Common;
using InvoiceAppWebApi.Domain;
using InvoiceAppWebApi.Services.ServiceBase;
using InvoiceAppWebApi.ViewModel;

namespace InvoiceAppWebApi.Services.Interface
{
    public interface IPaymentService : ICrudBase<Payment, PaymentViewModel>
    {
        Task<PaymentViewModel> GetPaymentByInvoiceIdAsync(long invoiceId, long customerId, params string[] includes);

        Task<ServiceResultModel<PaymentViewModel>> AddPaymentInvoiceCustomer(PaymentInputViewModel paymentInput);
    }
}
