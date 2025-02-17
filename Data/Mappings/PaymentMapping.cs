using InvoiceAppWebApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace InvoiceAppWebApi.Data.Mappings
{
    public class PaymentMapping
    {
        public PaymentMapping(EntityTypeBuilder<Payment> entityBuilder)
        {
            entityBuilder.ToTable("Payment", "PUB");

            entityBuilder
                .HasOne(p => p.Invoice)
                .WithMany(i => i.Payments)
                .HasForeignKey(p => p.InvoiceId)
                .IsRequired(false);

            entityBuilder
                .HasOne(p => p.Customer)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.CustomerId)
                .IsRequired(false);
        }
    }
}
