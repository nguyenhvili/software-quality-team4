using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StocksReporting.Domain.Email;
using StocksReporting.Domain.Email.ValueObjects;

namespace StocksReporting.Infrastructure.Persistance.Configuration;

public class EmailConfiguration : IEntityTypeConfiguration<Email>
{
    public void Configure(EntityTypeBuilder<Email> builder)
    {
        ConfigureEmailsTable(builder);
    }

    private void ConfigureEmailsTable(EntityTypeBuilder<Email> builder)
    {
        builder.ToTable(nameof(Email) + "s");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
            .HasColumnName(nameof(EmailId))
            .ValueGeneratedNever()
            .HasConversion(
                emailId => emailId.Value,
                emailId => EmailId.Create(emailId)
            );

        builder.OwnsOne(e => e.EmailValue, e =>
        {
            e.Property(p => p.Value)
                .HasColumnName("Email")
                .IsRequired();
        });
    }
}
