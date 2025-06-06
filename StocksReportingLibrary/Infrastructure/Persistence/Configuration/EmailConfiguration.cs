﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using StocksReportingLibrary.Domain.Email;
using StocksReportingLibrary.Domain.Email.ValueObjects;

namespace StocksReportingLibrary.Infrastructure.Persistence.Configuration;

public class EmailConfiguration : IEntityTypeConfiguration<Domain.Email.Email>
{
    public void Configure(EntityTypeBuilder<Domain.Email.Email> builder)
    {
        ConfigureEmailsTable(builder);
    }

    private void ConfigureEmailsTable(EntityTypeBuilder<Domain.Email.Email> builder)
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
