using StocksReportingLibrary.Domain.Common;
using StocksReportingLibrary.Domain.EmailAttachmentPath.ValueObjects;

namespace StocksReportingLibrary.Domain.EmailAttachmentPath;

public class EmailAttachmentPath : AggregateRoot<EmailAttachmentPathId>
{
    public string Path { get; private set; }

    private EmailAttachmentPath(string path)
    {
        Id = EmailAttachmentPathId.CreateUnique();
        Path = path;
    }

    public static EmailAttachmentPath Create(string attachmentPath)
    {
        if (string.IsNullOrWhiteSpace(attachmentPath))
        {
            throw new ArgumentException("The path is empty!");
        }

        return new EmailAttachmentPath(attachmentPath);
    }
}
