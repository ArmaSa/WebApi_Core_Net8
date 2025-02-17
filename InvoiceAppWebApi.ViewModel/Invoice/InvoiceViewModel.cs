using InvoiceAppWebApi.ViewModel.BaseModel;
using InvoiceAppWebApi.ViewModel.Customer;
using InvoiceAppWebApi.ViewModel.InvoiceItem;
using static InvoiceAppWebApi.Common.Enums;

namespace InvoiceAppWebApi.ViewModel.Invoice
{
    public class InvoiceViewModel : BaseViewModel
    {
        public string InvoiceNumber { get; set; }
        public DateTime SodorDate { get; set; }
        public DateTime? SarResidDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public PaymentStatus Status { get; set; }
        public int CustomerId { get; set; }
        public CustomerViewModel Customer { get; set; }

        public List<PaymentViewModel> Payments { get; set; } = new List<PaymentViewModel>();
        public List<InvoiceItemViewModel> Items { get; set; } = new List<InvoiceItemViewModel>();
    }
}
