using InvoiceProcessor.Domain.Vendors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceProcessor.Infrastructure.Persistence.Configurations;

public class VendorConfiguration : IEntityTypeConfiguration<Vendor>
{
    public void Configure(EntityTypeBuilder<Vendor> builder)
    {
        builder.HasKey(v => v.Id);
        
        builder.Property(v => v.Name).IsRequired().HasMaxLength(200);
        
        builder.Property(v => v.VatNumber).HasMaxLength(50);
        
        builder.HasOne(v=> v.DefoultCategory).WithMany().HasForeignKey(v => v.DefoultCategoryId).OnDelete(DeleteBehavior.SetNull);
        
    }
}