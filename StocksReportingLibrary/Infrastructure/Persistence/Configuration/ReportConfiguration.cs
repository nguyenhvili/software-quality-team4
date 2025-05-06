using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StocksReportingLibrary.Domain.Report;
using StocksReportingLibrary.Domain.Report.Holding;
using StocksReportingLibrary.Domain.Report.Holding.ValueObjects;
using StocksReportingLibrary.Domain.Report.ValueObjects;

namespace StocksReportingLibrary.Infrastructure.Persistence.Configuration;

public class ReportConfiguration : IEntityTypeConfiguration<Report>
{
    public void Configure(EntityTypeBuilder<Report> builder)
    {
        ConfigureReportsTable(builder);
        ConfigureHoldingTable(builder);
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

        builder.OwnsOne(r => r.ReportPathValue, r =>
        {
            r.Property(p => p.PathValue)
                .HasColumnName("FilePath")
                .IsRequired()
                .HasMaxLength(500);
        });

        builder.Property(r => r.CreatedAt)
            .IsRequired();
    }

    private void ConfigureHoldingTable(EntityTypeBuilder<Report> builder)
    {
        builder.OwnsMany(r => r.Holdings, hb =>
        {
            hb.ToTable(nameof(Holding) + "s");

            hb.HasKey(h => h.Id);

            hb.Property(h => h.Id)
                .HasColumnName(nameof(HoldingId))
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => HoldingId.Create(value)
                );

            hb.OwnsOne(h => h.Company, h =>
            {
                h.Property(p => p.Name)
                    .HasColumnName("Company");
            });

            hb.OwnsOne(h => h.Ticker, h =>
            {
                h.Property(p => p.Value)
                    .HasColumnName("Ticker");
            });

            hb.OwnsOne(h => h.Shares, h =>
            {
                h.Property(p => p.Value)
                    .HasColumnName("Shares");
            });

            hb.OwnsOne(h => h.SharesPercent, h =>
            {
                h.Property(p => p.Value)
                    .HasColumnName("SharesPercent")
                    .HasDefaultValue(0);
            });

            hb.OwnsOne(h => h.Weight, h =>
            {
                h.Property(p => p.Value)
                    .HasColumnName("Weight");
            });

            hb.WithOwner().HasForeignKey(nameof(ReportId));
        });
    }
}
