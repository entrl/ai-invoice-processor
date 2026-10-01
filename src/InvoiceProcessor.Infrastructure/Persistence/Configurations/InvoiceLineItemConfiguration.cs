using InvoiceProcessor.Domain.Invoices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceProcessor.Infrastructure.Persistence.Configurations;

public class InvoiceLineItemConfiguration : IEntityTypeConfiguration<InvoiceLineItem>
{
    public void Configure(EntityTypeBuilder<InvoiceLineItem> builder)
    {
        builder.HasKey(li => li.Id);
        
        builder.Property(li => li.Description).IsRequired().HasMaxLength(500);
        
        builder.Property(li => li.Quantity).HasPrecision(18, 4);
        builder.Property(li => li.UnitPrice).HasPrecision(18, 2);
        builder.Property(li => li.VatRate).HasPrecision(5, 4);
        builder.Property(li => li.LineTotal).HasPrecision(18, 2);
        
        builder.HasOne(li => li.Category).WithMany().HasForeignKey(li => li.CategoryId).OnDelete(DeleteBehavior.SetNull);
    }
}