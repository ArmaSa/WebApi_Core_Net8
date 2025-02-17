using InvoiceAppWebApi.Domain;
using InvoiceAppWebApi.Domain.BaseEntity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceAppWebApi.Data.Mappings
{
    public class AuditLogMapping
    {
        public AuditLogMapping(EntityTypeBuilder<AuditLog> entityBuilder)
        {
            entityBuilder.ToTable("AuditLog", "PUB")
                .OwnsOne(a => a.OldValues, b => { b.ToJson(); })
                .OwnsOne(a => a.NewValues, b => { b.ToJson(); })
                .Property(a => a.ChangedColumns)
                .HasConversion(
                v => string.Join(",", v),
                v => v.Split(",", StringSplitOptions.RemoveEmptyEntries).ToList()
                );
            entityBuilder.HasKey(e => e.Id);

            entityBuilder.HasIndex(e => e.EntityName);
            entityBuilder.Property(e => e.Id);

            entityBuilder.Property(e => e.ChangedUserId);
            entityBuilder.Property(e => e.EntityName).HasMaxLength(100).IsRequired();
            entityBuilder.Property(e => e.ChangeDate).IsRequired();

            entityBuilder.Property(e => e.AuditType).HasConversion<string>();

            entityBuilder.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.ChangedUserId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
