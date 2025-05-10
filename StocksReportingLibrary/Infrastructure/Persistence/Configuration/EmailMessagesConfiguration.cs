using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StocksReportingLibrary.Domain.Email.EmailMessage;
using StocksReportingLibrary.Domain.Email.EmailMessage.EmailAttachmentPath;
using StocksReportingLibrary.Domain.Email.EmailMessage.EmailAttachmentPath.ValueObjects;
using StocksReportingLibrary.Domain.Email.EmailMessage.Enums;
using StocksReportingLibrary.Domain.Email.EmailMessage.ValueObjects;

namespace StocksReportingLibrary.Infrastructure.Persistence.Configuration;

public sealed class EmailMessagesConfiguration
    : IEntityTypeConfiguration<EmailMessage>
{
    public void Configure(EntityTypeBuilder<EmailMessage> builder)
    {
        ConfigureEmailMessagesTable(builder);
    }

    private static void ConfigureEmailMessagesTable(
        EntityTypeBuilder<EmailMessage> builder)
    {
        builder.ToTable(nameof(EmailMessage) + "s");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id)
               .HasColumnName(nameof(EmailMessageId))
               .ValueGeneratedNever()
               .HasConversion(
                    id => id.Value,
                    value => EmailMessageId.Create(value));

        builder.Property(e => e.CreatedAt);

        builder.OwnsOne(e => e.EmailRecipient, r =>
        {
            r.Property(p => p.Value)
             .HasColumnName("Recipient")
             .IsRequired()
             .HasMaxLength(320);
        });

        builder.OwnsOne(e => e.EmailSubject, s =>
        {
            s.Property(p => p.Value)
             .HasColumnName("Subject")
             .IsRequired()
             .HasMaxLength(200);
        });

        builder.OwnsOne(e => e.EmailBody, b =>
        {
            b.Property(p => p.Value)
             .HasColumnName("Body")
             .IsRequired();
        });

        builder.OwnsMany(e => e.EmailAttachments, eb =>
        {
            eb.ToTable(nameof(EmailAttachmentPath) + "s");

            eb.HasKey(a => a.Id);

            eb.Property(a => a.Id)
                .HasColumnName(nameof(EmailAttachmentPathId))
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    value => EmailAttachmentPathId.Create(value));


            eb.Property(a => a.Path);
        });

        builder.Property(e => e.EmailSendStatus)
            .HasColumnName("Status")
            .IsRequired()
            .HasConversion(
                status => (int)status.Status,
                status => EmailSendStatus.Create((EEmailSendStatus)status));

        builder.OwnsOne(e => e.RetryCount, rc =>
        {
            rc.Property(p => p.Value)
              .HasColumnName("RetryCount")
              .IsRequired();
        });

        builder.HasIndex(e => e.EmailSendStatus);
    }
}
