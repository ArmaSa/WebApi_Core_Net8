using InvoiceAppWebApi.FrameworkExtention;
using static InvoiceAppWebApi.Common.Enums;

namespace InvoiceAppWebApi.Domain
{
    public class Invoice : BaseEntity.BaseEntity
    {
        public string InvoiceNumber { get; set; }
        public DateTime SodorDate { get; set; } 
        public DateTime? SarResidDate { get; set; } 
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; } 
        public PaymentStatus Status { get; set; } 
        public long CustomerId { get; set; } 
        public Customer Customer { get; set; }
        public virtual ICollection<Payment>? Payments { get; set; } = new List<Payment>();
        public virtual ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    }
}
