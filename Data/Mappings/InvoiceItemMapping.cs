using InvoiceAppWebApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace InvoiceAppWebApi.Data.Mappings
{
    public class InvoiceItemMapping
    {
        public InvoiceItemMapping(EntityTypeBuilder<InvoiceItem> entityBuilder)
        {
            entityBuilder.ToTable("InvoiceItem", "PUB");

            entityBuilder
                .Property(ii => ii.Description)
                .HasMaxLength(200)
                .IsRequired();
        }
    }
}
