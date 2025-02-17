using System.ComponentModel.DataAnnotations;
using static InvoiceAppWebApi.Common.Enums;

namespace InvoiceAppWebApi.Domain.BaseEntity
{
    public class AuditLog
    {
        public AuditLog() { }

        [Key]
        public long Id { get; set; }
        public string? ChangedUserId { get; set; }

        public ApplicationUser? User { get; set; }

        public TrailType AuditType { get; set; }

        public required string EntityName { get; set; }

        public string? OldValues { get; set; }

        public string? NewValues { get; set; }

        public List<string> ChangedColumns { get; set; } = [];

        public DateTime ChangeDate { get; set; } = DateTime.UtcNow;
    }
}
