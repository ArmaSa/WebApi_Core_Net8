using static InvoiceAppWebApi.Common.Enums;
using InvoiceAppWebApi.Domain.BaseEntity;

namespace InvoiceAppWebApi.Domain
{
    public class Payment : BaseEntity.BaseEntity
    {
        public long InvoiceId { get; set; }
        public long CustomerId { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; } 
        public PaymentMethods PaymentMethod { get; set; }
        public Invoice? Invoice { get; set; }
        public Customer? Customer { get; set; }
    }
}
