using InvoiceAppWebApi.FrameworkExtention;
using System.ComponentModel.DataAnnotations;

namespace InvoiceAppWebApi.Domain.BaseEntity
{
    public class BaseEntity : AuditEntity, IKey64
    {
        [Key]
        public long Id { get; set; }
    }
}
