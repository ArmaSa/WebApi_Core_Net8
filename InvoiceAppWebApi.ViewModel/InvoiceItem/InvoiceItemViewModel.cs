using InvoiceAppWebApi.ViewModel.BaseModel;
using InvoiceAppWebApi.ViewModel.Invoice;

namespace InvoiceAppWebApi.ViewModel.InvoiceItem
{
    public class InvoiceItemViewModel : BaseViewModel
    {
        public int InvoiceId { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
        public InvoiceViewModel Invoice { get; set; }
    }
}
