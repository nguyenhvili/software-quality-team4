using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StocksReporting.Domain.Report;
using StocksReporting.Domain.Report.ValueObjects;

namespace StocksReporting.Infrastructure.Persistence.Configuration;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        ConfigureReportsTable(builder);
    }

    private void ConfigureReportsTable(EntityTypeBuilder<Report> builder)
    {
        builder.ToTable(nameof(Report) + "s");

        builder.HasKey(r => r.Id);

        builder.Property(r => r.Id)
            .HasColumnName(nameof(ReportId))
            .ValueGeneratedNever()
            .HasConversion(
                id => id.Value,
                value => ReportId.Create(value)
            );

        builder.Property(r => r.FilePath)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(r => r.CreatedAt)
            .IsRequired();
    }
}
