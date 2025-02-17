namespace InvoiceAppWebApi.Domain
{
    public class InvoiceItem : BaseEntity.BaseEntity
    {
        public long InvoiceId { get; set; }
        public string Description { get; set; } 
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; } 
        public decimal TotalPrice => Quantity * UnitPrice;
        public Invoice Invoice { get; set; }
    }
}
