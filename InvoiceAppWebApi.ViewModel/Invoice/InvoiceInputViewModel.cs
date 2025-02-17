using System.ComponentModel.DataAnnotations;
using static InvoiceAppWebApi.Common.Enums;

namespace InvoiceAppWebApi.ViewModel.Invoice
{
    public class InvoiceInputViewModel
    {
        [Required]
        public DateTime SodorDate { get; set; }
        public DateTime? SarResidDate { get; set; }
        [Required]
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public PaymentStatus Status { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public List<InvoiceItemInputViewModel> InvoiceItem { get; set; }
    }

    public class InvoiceItemInputViewModel
    {
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
