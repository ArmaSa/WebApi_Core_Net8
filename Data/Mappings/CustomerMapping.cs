using InvoiceAppWebApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceAppWebApi.Data.Mappings
{
    public class CustomerMapping
    {
        public CustomerMapping(EntityTypeBuilder<Customer> entityBuilder)
        {
            entityBuilder.ToTable("Customer", "PUB");

            entityBuilder
                .HasMany(i => i.Payments)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            entityBuilder
                .HasMany(i => i.Invoices)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Cascade);

            entityBuilder
                .Property(c => c.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entityBuilder
                .Property(c => c.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entityBuilder
                .Property(c => c.Email)
                .HasMaxLength(100)
                .IsRequired();
        }
    }
}
