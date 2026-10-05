using InvoiceProcessor.Domain.Invoices;
using InvoiceProcessor.Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceProcessor.Infrastructure.Persistence.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
    public void Configure(EntityTypeBuilder<Invoice> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.InvoiceNumber).IsRequired().HasMaxLength(100);

        builder.Property(i => i.Currency).IsRequired().HasMaxLength(3);

        builder.Property(i => i.FilePath);

        builder.Property(i => i.Subtotal).HasPrecision(18, 2);
        builder.Property(i => i.VatAmount).HasPrecision(18, 2);
        builder.Property(i => i.Total).HasPrecision(18, 2);

        builder.Property(i => i.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasOne(i => i.Vendor)
            .WithMany()
            .HasForeignKey(i => i.VendorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<User>()
            .WithMany()
            .HasForeignKey(i => i.UploadedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(i => i.LineItems)
            .WithOne()
            .HasForeignKey(i => i.InvoiceId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(i => new { i.VendorId, i.InvoiceNumber }).IsUnique();
    }
}