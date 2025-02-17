using InvoiceAppWebApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceAppWebApi.Data.Mappings
{
    public class InvoiceMapping
    {
        public InvoiceMapping(EntityTypeBuilder<Invoice> entityBuilder)
        {
            entityBuilder.ToTable("Invoice", "PUB");

            entityBuilder
                .HasOne(i => i.Customer)
                .WithMany(i => i.Invoices)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            entityBuilder
                .HasMany(i => i.Items)
                .WithOne(ii => ii.Invoice)
                .HasForeignKey(ii => ii.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entityBuilder
                .HasMany(i => i.Payments)
                .WithOne(p => p.Invoice)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            entityBuilder
                .Property(i => i.InvoiceNumber)
                .HasMaxLength(50)
                .IsRequired();
        }
    }
}