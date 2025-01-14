using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Azure.Communication.Email;
using Microsoft.Extensions.Options;

namespace RoosterPlanner.Email;


public class ACSConfig
{
    public string ConnectionString { get; set; }
    public string SenderEmail { get; set; }
}

public class AzureEmailServicesClient(IOptions<ACSConfig> options, EmailClient emailClient) : IEmailService
{
    private readonly ACSConfig _config = options.Value;
    private readonly EmailClient _emailClient = emailClient;

    public Task SendEmail(string recipient, string subject, string body, bool isBodyHtml, string sender, Attachment attachment)
    {
        return SendEmail([recipient], subject, body, isBodyHtml, sender, attachment);
    }

    public Task SendEmail(IEnumerable<string> recipients, string subject, string body, bool isBodyHtml, string sender, Attachment attachment)
    {
        var emailMessage = new EmailMessage(
            _config.SenderEmail,
            new EmailRecipients(recipients.Select(r => new EmailAddress(r))),
            new(subject)
            {
                Html = body,
            }
        );

        if (attachment != null)
        {
            using var memoryStream = new System.IO.MemoryStream();
            attachment.ContentStream.CopyTo(memoryStream);
            var attachmentBytes = memoryStream.ToArray();
            emailMessage.Attachments.Add(new EmailAttachment(attachment.Name, attachment.ContentType.ToString(), new(attachmentBytes)));
        }

        return _emailClient.SendAsync(Azure.WaitUntil.Completed, emailMessage);
    }
}