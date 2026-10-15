using InvoiceProcessor.Domain.Anomalies;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace InvoiceProcessor.Infrastructure.Persistence.Configurations;

public class AnomalyFlagConfiguration : IEntityTypeConfiguration<AnomalyFlag>
{
    public void Configure(EntityTypeBuilder<AnomalyFlag> builder)
    {
        builder.HasKey(f => f.Id);
        
        builder.Property(f => f.Code).IsRequired().HasMaxLength(50);
        builder.Property(f => f.Reason).IsRequired().HasMaxLength(500);
        builder.Property(f => f.Severity).HasConversion<string>().HasMaxLength(20);
    }
}