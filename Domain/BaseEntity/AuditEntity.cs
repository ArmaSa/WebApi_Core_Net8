using static InvoiceAppWebApi.Common.Enums;

namespace InvoiceAppWebApi.Domain.BaseEntity
{
    public class AuditEntity
    {
        public string? UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
